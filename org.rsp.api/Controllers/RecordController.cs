using System.Net;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.api.Auth;


namespace org.rsp.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RecordController
{
    private readonly IRecordManager _recordManager;
    private readonly ILogger<RecordController> _logger;

    public RecordController(IRecordManager recordManager, ILogger<RecordController> logger)
    {
        _recordManager = recordManager;
        _logger = logger;
    }

    /// <summary>
    /// 添加出库/入库记录
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    public async Task<AddRecordResponse> AddWareHouseRecord(AddWareHouseRecordRequest request)
    {
        try
        {
            return await _recordManager.AddRecordAsync(request);
        }
        catch (Exception e) 
        {
            _logger.LogError($"{e.Message}");
            return new AddRecordResponse
            {
                Message = "服务内部异常，请联系管理员",
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// 查询流水
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ResponseResult<QueryRecordResponse>> QueryRecord(QueryRecordRequest request)
    {
        return await _recordManager.QueryRecordAsync(request);
    }

    /// <summary>
    /// 删除流水
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task BatchDelRecord(List<int> ids)
    {
        await _recordManager.BatchDelRecordAsync(ids);
    }

    
    [HttpPost]
    [Yes(Roles = "Administrator,Regular")]
    [ProducesResponseType(typeof(byte[]), 200, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> ExportRecord()
    {
        ExcelPackage.LicenseContext=LicenseContext.NonCommercial;

        var memoryStream = new MemoryStream();
        using var excelPackage = new ExcelPackage(memoryStream);
        var worksheet = excelPackage.Workbook.Worksheets.Add("出入库记录");
        worksheet.Cells[1, 1].Value = "商品名";
        worksheet.Cells[1,2].Value ="仓库";
        worksheet.Cells[1,3].Value ="进/出库";
        worksheet.Cells[1,4].Value ="数量";
        worksheet.Cells[1,5].Value ="用途";
        worksheet.Cells[1,6].Value ="交易时间";
        worksheet.Cells[1,7].Value ="创建人";

        var result =await _recordManager.QueryRecordAsync(new QueryRecordRequest());
        int row = 2;
        foreach (var recordModel in result.RecordModels)
        {
            worksheet.Cells[row, 1].Value = recordModel.GoodsName;
            worksheet.Cells[row, 2].Value = recordModel.StoreHouseName;
            worksheet.Cells[row, 3].Value = recordModel.DirectionName;
            worksheet.Cells[row, 4].Value = recordModel.Quantity;
            worksheet.Cells[row, 5].Value = recordModel.Use;
            worksheet.Cells[row, 6].Value = recordModel.TradeTime.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row,7].Value =recordModel.CreateBy;
            row++;
        }

        await excelPackage.SaveAsync();
        memoryStream.Position = 0;
        return new FileContentResult(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = "example.xlsx"
        };
        
    }


    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task UpdateRecord(UpdateRecordRequest request)
    {
        try
        {
            await _recordManager.UpdateRecordAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError($"UpdateRecordRequest:  {e.Message}");
            throw;
        }
    }
}