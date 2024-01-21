# 仓库集合
$sunblog_register = 'registry.cn-hangzhou.aliyuncs.com/learn-zhou/zhou-learn'


# 应用镜像集合
$apptags = [System.Collections.ArrayList]::new()
$count = $apptags.Add("hostblog")
$count = $apptags.Add("vueblog")
$count = $apptags.Add("adminvue")

Write-Host "导出任务已开始"

Write-Host "确认已经登录镜像仓库"

Write-Host "正在导出sunblog系统镜像...."
# 开始导出应用镜像
for ($i = 0; $i -lt $apptags.Count; $i++) {
    # 拼接名称    
    $imgName = $sunblog_register + ':' + $apptags[$i]

    $targetImgName = $sunblog_register + ':' + $apptags[$i]

    Write-Host ("正在导出: 本地 到: " + $targetImgName)

    # docker pull $imgName

    # docker tag $imgName $targetImgName

    docker push $targetImgName
    # 删除
    # docker rmi $imgName
    # docker rmi $targetImgName
}
Write-Host "导出任务已结束"

