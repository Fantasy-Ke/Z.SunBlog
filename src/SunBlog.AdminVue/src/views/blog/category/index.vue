<template>
	<div class="blog-category-container layout-padding">
		<ProTable ref="tableRef" :request-api="getTableList" :data-callback="dataCallBack" :pagination="false" :columns="columns" :tool-button="false">
			<template #tools>
				<!-- v-auth="'category:add'"  -->
				<el-button type="primary" icon="ele-Plus" @click="onOpenDialog(null)"> 新增 </el-button>
			</template>
			<template #status="scope">
				<el-tag :type="scope.row.status === 0 ? 'success' : 'danger'"> {{ scope.row.status === 0 ? '启用' : '禁用' }}</el-tag>
			</template>
			<!-- v-auth="'category:edit'"  -->
			<template #action="scope">
				<el-button icon="ele-Edit" size="small" text type="primary" @click="onOpenDialog(scope.row)"> 编辑 </el-button>
				<el-popconfirm title="确认删除吗？" @confirm="onDelete(scope.row.id)">
					<template #reference>
						<!-- v-auth="'category:delete'" -->
						<el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
					</template>
				</el-popconfirm>
			</template>
		</ProTable>
		<CategoryDialog ref="categoryDialogRef" @refresh="tableRef?.reset" />
	</div>
</template>

<script setup lang="ts" name="blogCategory">
import { defineAsyncComponent, ref, reactive, inject } from 'vue';
import { ElMessage } from 'element-plus';

// 引入组件
const CategoryDialog = defineAsyncComponent(() => import('./dialog.vue'));
import ProTable from '@/components/ProTable/index.vue';
import type { ColumnProps } from '@/components/ProTable/interface';
import { CategorySsServiceProxy, KeyDto, UpdateCategoryInput } from '@/shared/service-proxies';
import moment from 'moment';

const _categoryService = new CategorySsServiceProxy(inject('$baseurl'), inject('$api'));

// 定义变量内容
const categoryDialogRef = ref<InstanceType<typeof CategoryDialog>>();
const tableRef = ref<InstanceType<typeof ProTable>>();

const columns = reactive<ColumnProps[]>([
	{
		prop: 'name',
		label: '栏目名称',
		search: { el: 'input' },
		align: 'left',
	},
	{
		prop: 'status',
		label: '状态',
	},
	{
		prop: 'sort',
		label: '排序',
	},
	{
		prop: 'creationTime',
		label: '创建时间',
	},
	{
		prop: 'action',
		label: '操作',
		width: 150,
		// isShow: auths(['category:edit', 'category:delete']),
	},
]);

const dataCallBack = (data) => {
	data.forEach((res) => {
		if (res.creationTime) {
			res.creationTime = moment(res.creationTime).format('YYYY-MM-DD');
		}
	});

	return data;
};

const getTableList = (params: any) => {
	let newParams = JSON.parse(JSON.stringify(params));
	return _categoryService.getPage(newParams.name);
};

// 打开新增栏目弹窗
const onOpenDialog = async (row: UpdateCategoryInput | null = null) => {
	await categoryDialogRef.value?.openDialog(row);
};

//删除机构
const onDelete = async (id: string) => {
	const { success } = await _categoryService.delete({ id: id } as KeyDto);
	if (success) {
		ElMessage.success('删除成功');
		tableRef.value?.reset();
	}
};
</script>
