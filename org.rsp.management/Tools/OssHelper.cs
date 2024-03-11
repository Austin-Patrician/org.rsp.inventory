using Aliyun.OSS;

namespace org.rsp.management.Tools;

public static class OssHelper
{
    /// <summary>
    /// 分片上传
    /// </summary>
    public async static Task UploadFileToOss()
    {
        var endpoint = "yourEndpoint";
        // 从环境变量中获取访问凭证。运行本代码示例之前，请确保已设置环境变量OSS_ACCESS_KEY_ID和OSS_ACCESS_KEY_SECRET。
        var accessKeyId = Environment.GetEnvironmentVariable("OSS_ACCESS_KEY_ID");
        var accessKeySecret = Environment.GetEnvironmentVariable("OSS_ACCESS_KEY_SECRET");
        // 填写Bucket名称。
        var bucketName = "examplebucket";
        // 填写Object完整路径。Object完整路径中不能包含Bucket名称。
        var objectName = "exampleobject.txt";
        // 填写本地文件的完整路径。如果未指定本地路径，则默认从示例程序所属项目对应本地路径中上传文件。
        var localFilename = "D:\\localpath\\examplefile.txt";
        // 创建OssClient实例。
        var client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        // 初始化分片上传，返回uploadId。
        var uploadId = "";
        
        
        try
        {
            // 定义上传的文件及所属Bucket的名称。您可以在InitiateMultipartUploadRequest中设置ObjectMeta，但不必指定其中的ContentLength。
            var request = new InitiateMultipartUploadRequest(bucketName, objectName);
            var result = client.InitiateMultipartUpload(request);
            uploadId = result.UploadId;
            // 打印UploadId。
            Console.WriteLine("Init multi part upload succeeded");
            Console.WriteLine("Upload Id:{0}", result.UploadId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Init multi part upload failed, {0}", ex.Message);
        }

        // 计算分片总数。
        var partSize = 100 * 1024;
        var fi = new FileInfo(localFilename);
        var fileSize = fi.Length;
        var partCount = fileSize / partSize;
        if (fileSize % partSize != 0)
        {
            partCount++;
        }

        // 开始分片上传。PartETags是保存PartETag的列表，OSS收到用户提交的分片列表后，会逐一验证每个分片数据的有效性。当所有的数据分片通过验证后，OSS会将这些分片组合成一个完整的文件。
        var partETags = new List<PartETag>();
        try
        {
            using (var fs = File.Open(localFilename, FileMode.Open))
            {
                for (var i = 0; i < partCount; i++)
                {
                    var skipBytes = (long)partSize * i;
                    // 定位到本次上传的起始位置。
                    fs.Seek(skipBytes, 0);
                    // 计算本次上传的分片大小，最后一片为剩余的数据大小。
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, objectName, uploadId)
                    {
                        InputStream = fs,
                        PartSize = size,
                        PartNumber = i + 1
                    };
                    // 调用UploadPart接口执行上传功能，返回结果中包含了这个数据片的ETag值。
                    var result = client.UploadPart(request);
                    partETags.Add(result.PartETag);
                    Console.WriteLine("finish {0}/{1}", partETags.Count, partCount);
                }

                Console.WriteLine("Put multi part upload succeeded");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Put multi part upload failed, {0}", ex.Message);
        }

        // 完成分片上传。
        try
        {
            var completeMultipartUploadRequest = new CompleteMultipartUploadRequest(bucketName, objectName, uploadId);
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }

            var result = client.CompleteMultipartUpload(completeMultipartUploadRequest);
            Console.WriteLine("complete multi part succeeded");
            Console.WriteLine(result.Location);
        }
        catch (Exception ex)
        {
            Console.WriteLine("complete multi part failed, {0}", ex.Message);
        }
    }
}