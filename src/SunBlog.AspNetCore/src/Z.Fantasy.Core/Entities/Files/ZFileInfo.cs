using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Extensions;

namespace Z.Fantasy.Core.Entities.Files
{
    public class ZFileInfo : FullAuditedEntity<Guid>
    {

        public FileType _fileType;

        #region 计算code长度

        /// <summary>
        /// Maximum length of the <see cref="Code" /> property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// Maximum depth of an UO hierarchy.
        /// </summary>
        public const int MaxDepth = 16;
        #endregion
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
        public FileType FileType
        {
            get
            {
                return _fileType;
            }
            set
            {
                _fileType = value;
                FileTypeString = _fileType.ToNameValue();
            }
        }

        /// <summary>
        /// 文件类型名称
        /// </summary>
        public string FileTypeString { get; set; }

        /// <summary>
        /// 编码  (记录文件的层次结构关系)
        /// Example: "00001.00042.00005". 这是租户的唯一代码。 当然可以进行修改
        /// </summary>
        /// 
        [Required]
        [StringLength(MaxCodeLength)]
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
        /// 将子代码附加到父代码。 例如:如果parentCode = "00001"，则childCode = "00042"，然后返回"00001.00042"。
        /// </summary>
        /// <param name="parentCode"> 父类的代码。 如果父节点是根节点，则可以为空或空。 </param>
        /// <param name="childCode"> 子代码. </param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "子代码不能为空或者为null");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// 为给定的数字创建代码。
        /// Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers"> Numbers </param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// 生成计算给定代码的下一个代码。
        /// Example: if code = "00019.00055.00001" 返回 "00019.00055.00002".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// 获取当前父级code
        /// Example: if code = "00019.00055.00001" returns "00019.00055".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        /// <summary>
        /// 获取code最小的那个.
        /// Example: if code = "00019.00055.00001" returns "00001".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// 传入父子code 获取去除父级得code代码.
        /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// </summary>
        /// <param name="code"> The code. </param>
        /// <param name="parentCode"> The parent code. </param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }
    }

}
