# OpendataLCMR營收查詢系統

本專案是一個整合前後端的營收查詢系統，使用開放資料來源，每次啟動可以手動點擊匯入最新資料，並提供篩選查詢功能。

此專案為個人練習用途，主要練習以下技術：

## 架構技術
- **後端：.NET 6**
  - ASP.NET Core Web API
  - MSSQL資料庫
  - 預存程序 (Stored Procedure) 操作資料庫
  - Swagger
- **前端：Vue 3**
  - 資料查詢、重新匯入功能

### 資料庫
1. 匯入資料庫結構：
   使用 `doc/db-schema.sql` 匯入 MSSQL 資料庫。

2. 確認 `appsettings.json` 連線字串正確。

### 前端 (Vue 3)
1. 前端會在 `http://localhost:8080` 啟動。

### 後端 (.NET API)
1. 啟動後可使用 Swagger 介面測試 API：
   - `https://localhost:7243/swagger`

## API 及 前端 功能說明

### 前端 (Vue 3)
1. 進入即查詢資料
     
2. 重新匯入按鈕可重新更新資料，並重新查詢，每`5分鐘`能重新匯入
   
3. 可用`公司代`號以及`年月`查詢營收資料
   
### GET /query/query
查詢營收資料(原生 SQL)
參數：`dataYYYMM`, `companyCode`（皆為選填）

### GET /query/querySp
查詢營收資料(EF Core)
參數：`dataYYYMM`, `companyCode`（皆為選填）

### POST /query/reloadRevenueData
重新從開放平台匯入資料(Marge)


## 🌐 資料來源

- [臺灣證券交易所：每月營收彙總表](https://mopsfin.twse.com.tw/opendata/t187ap05_L.csv)
