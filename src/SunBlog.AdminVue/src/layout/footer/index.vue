<template>
	<div class="layout-footer pb15">
		<div class="layout-footer-warp">
			<div> SunBlog Admin❤️</div>
			<div class="mt5">广州市 广州市天河区天河路1号</div>
			<div class="mt5">Copyright © 2021-2022 SunBlog Admin. All Rights Reserved.</div>
		</div>
	</div>
</template>

<script setup lang="ts" name="layoutFooter">
// 此处需有内容（注释也得），否则缓存将失败
</script>

<style scoped lang="scss">
.layout-footer {
	width: 100%;
	display: flex;
	&-warp {
		margin: auto;
		color: var(--el-text-color-secondary);
		text-align: center;
		animation: error-num 0.3s ease;
	}
}
</style>
