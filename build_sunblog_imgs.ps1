# 仓库集合
$sunblog_register = 'registry.cn-hangzhou.aliyuncs.com/learn-zhou/zhou-learn/'.TrimEnd('/')

function WriteNewLine ($msg) {
    Write-Host "\r\n$msg\r\n"
}

# 获取标签
# 应用镜像集合
$apptags = [System.Collections.ArrayList]::new()
$count = $apptags.Add("hostblog")
$count = $apptags.Add("vueblog")
$count = $apptags.Add("adminvue")

# 打包镜像
WriteNewLine '开始编译镜像，请耐心等待...'

# $tag = ''
# while ($True) {
#     Write-Host '请输入标签'
#     $inputTag = [System.Console]::ReadLine()
#     if ([System.String]::IsNullOrWhiteSpace($inputTag)) {
#         Write-Host '不能输入空值!' 
#     }
#     else {
#         $tag = $inputTag.Trim()
#         break
#     }
# }
# ## host
Set-Location ./src/SunBlog.AspNetCore
$imgName = $sunblog_register + ':' + $apptags[0]
WriteNewLine "正在编译 hostblog 镜像： $imgName"
docker build . --force-rm -t $imgName  -f ./Z.SunBlog.Host/Dockerfile
## 推送
# docker push $imgName

# ## migrator
Set-Location ../SunBlog.BlogVue
$imgName = $sunblog_register  + ':' + $apptags[1]
WriteNewLine "正在编译 vueblog 镜像： $imgName"
docker build . --force-rm -t $imgName  -f Dockerfile
## 推送
# docker push $imgName

## ui
Set-Location ../SunBlog.AdminVue
$imgName = $sunblog_register + ':' + $apptags[2]
WriteNewLine "正在编译 adminvue 镜像： $imgName"
docker build . --force-rm -t $imgName -f Dockerfile
## 推送
# docker push $imgName
Set-Location ..



# 打包镜像结束
WriteNewLine '镜像编译已结束，请检查是否镜像是否正常！'
