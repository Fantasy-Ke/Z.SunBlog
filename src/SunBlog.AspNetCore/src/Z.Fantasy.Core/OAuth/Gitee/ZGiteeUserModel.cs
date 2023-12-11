using MrHuo.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.OAuth.Gitee
{
    public class ZGiteeUserModel : IUserInfoModel, IUserInfoErrorModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("avatar_url")]
        public string Avatar { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("blog")]
        public string Blog { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreateAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdateAt { get; set; }

        [JsonPropertyName("public_repos")]
        public int PublicRepos { get; set; }

        [JsonPropertyName("public_gists")]
        public int PublicGists { get; set; }

        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        [JsonPropertyName("following")]
        public int Following { get; set; }

        [JsonPropertyName("stared")]
        public int Stared { get; set; }
        [JsonPropertyName("watched")]
        public int Watched { get; set; }

        [JsonPropertyName("message")]
        public string ErrorMessage { get; set; }
    }
}
