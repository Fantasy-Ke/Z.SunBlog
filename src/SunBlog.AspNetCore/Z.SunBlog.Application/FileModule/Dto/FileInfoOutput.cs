using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.FileModule.Dto;

public class FileInfoOutput
{
    public Guid Id { get; set; }
    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid? ParentId { get; set; }
    /// <summary>
    /// 文件显示名称，例如文件名为  open.jpg，显示名称为： open_编码规则
    /// </summary>
    public string FileDisplayName { get; set; }

    /// <summary>
    /// 文件原始名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string FileExt { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 文件大小，字节
    /// </summary>
    public string FileSize { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public FileType FileType { get; set; }

    /// <summary>
    /// 文件类型名称
    /// </summary>
    public string FileTypeString { get; set; }

    /// <summary>
    /// 编码  (记录文件的层次结构关系)
    /// Example: "00001.00042.00005". 这是租户的唯一代码。 当然可以进行修改
    /// </summary>
    /// 
    public virtual string Code { get; set; }

    /// <summary>
    /// 是否是文件夹
    /// </summary>
    public bool IsFolder { get; private set; }

    /// <summary>
    /// Ip 地址
    /// </summary>
    public string FileIpAddress { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreationTime { get; set; }
}