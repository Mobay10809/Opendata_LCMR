<template>
  <div class="p-4">
    <h1 class="text-xl mb-4">營收資料查詢</h1>

    <button :disabled="isReloading" @click="handleReload"
      class="px-4 py-2 bg-blue-500 text-white rounded disabled:opacity-50 disabled:cursor-not-allowed">
      {{ isReloading ? `重新整理中... (${countdown} 秒)` : '重新匯入資料' }}
    </button>
    <span style="color: red;" v-show="isReloading">倒數完才可再重新載入</span>

    <div>
      <h2>資料列表：</h2>
      <div class="form-row">
        <label for="dataYYYMM">資料年月:</label>
        <input id="dataYYYMM" type="number" v-model="dataYYYMM" placeholder="YYYMM" />
        <p style="color: red;" v-if="dataYYYMMError">{{ dataYYYMMError }}</p>
      </div>

      <div class="form-row">
        <label for="companyCode">公司代號:</label>
        <input id="companyCode" type="text" v-model="companyCode" />
      </div>
      <button @click="resetAndFetchData" style="margin: 10px 0px;">
        查詢
      </button>

      <div v-show="!loading"><label>總筆數:{{ totalRecords }}</label></div>
      <div v-if="loading">資料載入中...</div>
      <table v-else border="1" cellpadding="5" cellspacing="0" style="justify-self: center;  overflow-x: auto;">
        <thead>
          <tr>
            <th>報表日期</th>
            <th>資料年月</th>
            <th>公司代號</th>
            <th>公司名稱</th>
            <th>產業類別</th>
            <th>當月營收</th>
            <th>上月營收</th>
            <th>去年當月營收</th>
            <th>月增率 (%)</th>
            <th>年增率 (%)</th>
            <th>累計營收</th>
            <th>去年累計營收</th>
            <th>累計增率 (%)</th>
            <th>備註</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in data" :key="item.id">
            <td>{{ item.reportDate }}</td>
            <td>{{ item.dataYYYMM }}</td>
            <td>{{ item.companyCode }}</td>
            <td>{{ item.companyName }}</td>
            <td>{{ item.industry }}</td>
            <td>{{ item.currentRevenue }}</td>
            <td>{{ item.lastMonthRevenue }}</td>
            <td>{{ item.lastYearSameMonthRevenue }}</td>
            <td>{{ item.monthlyChangePercentage }}</td>
            <td>{{ item.yearlyChangePercentage }}</td>
            <td>{{ item.cumulativeCurrentRevenue }}</td>
            <td>{{ item.cumulativeLastYearRevenue }}</td>
            <td>{{ item.cumulativeChangePercentage }}</td>
            <td>{{ item.remark }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { ref, watch, onMounted, onBeforeUnmount } from 'vue'

export default {
  setup() {
    const data = ref([])
    const loading = ref(false)
    const isReloading = ref(false)
    const countdown = ref(300)
    const dataYYYMM = ref('')
    const companyCode = ref('')
    const totalRecords = ref(0)
    const dataYYYMMError = ref('')

    const page = ref(1)
    const pageSize = 100
    const isLoadingMore = ref(false)
    const hasMoreData = ref(true)

    let timer = null

    const fetchData = async () => {
      if (isLoadingMore.value || !hasMoreData.value) return
      isLoadingMore.value = true
      loading.value = true

      try {
        const params = new URLSearchParams()
        if (dataYYYMM.value) params.append('dataYYYMM', dataYYYMM.value)
        if (companyCode.value) params.append('companyCode', companyCode.value)
        params.append('page', page.value)
        params.append('pageSize', pageSize)

        const response = await fetch(`https://localhost:7243/query/queryEF_paged?${params.toString()}`)
        const result = await response.json()

        if (result.length < pageSize) {
          hasMoreData.value = false
        }

        data.value.push(...result.data)
        totalRecords.value = result.total
        console.log(data.value)
        console.log(totalRecords.value)
        page.value += 1
      } catch (error) {
        console.error('讀取資料失敗', error)
      } finally {
        loading.value = false
        isLoadingMore.value = false
      }
    }

    const resetAndFetchData = () => {
      page.value = 1
      data.value = []
      hasMoreData.value = true
      fetchData()
    }

    const handleReload = async () => {
      isReloading.value = true
      countdown.value = 300

      try {
        await fetch('https://localhost:7243/query/reloadRevenueData', { method: 'POST' })
        resetAndFetchData()
      } catch (error) {
        console.error('重新整理失敗', error)
      }

      timer = setInterval(() => {
        countdown.value--
        if (countdown.value <= 0) {
          clearInterval(timer)
          isReloading.value = false
        }
      }, 1000)
    }

    watch(dataYYYMM, (newVal) => {
      const valStr = String(newVal)
      if (valStr.length > 5) {
        dataYYYMM.value = valStr.slice(0, 5)
        dataYYYMMError.value = '資料年月不能超過 5 碼（YYYYM）'
      } else {
        dataYYYMMError.value = ''
      }
    })

    const handleScroll = () => {
      const scrollTop = window.scrollY
      const windowHeight = window.innerHeight
      const fullHeight = document.documentElement.scrollHeight

      if (scrollTop + windowHeight >= fullHeight - 50) {
        fetchData()
      }
    }

    onMounted(() => {
      window.addEventListener('scroll', handleScroll) //監聽畫面滾動
      resetAndFetchData()
    })

    onBeforeUnmount(() => {
      window.removeEventListener('scroll', handleScroll)
      if (timer) {
        clearInterval(timer)
      }
    })

    return {
      data,
      loading,
      isReloading,
      countdown,
      fetchData,
      dataYYYMM,
      companyCode,
      totalRecords,
      dataYYYMMError,
      handleReload,
      resetAndFetchData
    }
  }
}
</script>
