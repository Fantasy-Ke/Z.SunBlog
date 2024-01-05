<template>
	<div class="blog-account-container layout-padding">
		<ProTable ref="tableRef" :request-api="getTableList" :data-callback="dataCallBack" :columns="columns" :tool-button="false">
			<template #gender="{ row }">
				{{ row.gender === 0 ? '男' : row.gender === 1 ? '女' : '未知' }}
			</template>
			<template #isBlogger="{ row }">
				<!-- v-if="auth('authaccount:setblogger')" -->
				<el-switch v-model="row.isBlogger" inline-prompt active-text="是" inactive-text="否" @change="onChange(row.id)" />
				<!-- <el-tag :type="row.isBlogger ? 'success' : 'danger'">{{ row.isBlogger ? '是' : '否' }}</el-tag> -->
			</template>
			<template #action="scope">
				<el-popconfirm title="确认删除吗？" @confirm="onDeleteRole(scope.row.id)">
					<template #reference>
						<!-- v-auth="'authaccount:delete'" -->
						<el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
					</template>
				</el-popconfirm>
			</template>
		</ProTable>
	</div>
</template>

<script setup lang="ts" name="friendLink">
import { inject, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import ProTable from '@/components/ProTable/index.vue';
import { ColumnProps } from '@/components/ProTable/interface';
import { AuthAccountPageQueryInput, AuthAccountsServiceProxy } from '@/shared/service-proxies';
import moment from 'moment';
const _authsService = new AuthAccountsServiceProxy(inject('$baseurl'), inject('$api'));
const loading = ref(false);
//  table实例
const tableRef = ref<InstanceType<typeof ProTable>>();
const columns = reactive<ColumnProps[]>([
	{
		type: 'index',
		label: '序号',
		width: 60,
	},
	{
		prop: 'name',
		label: '昵称',
		search: { el: 'input' },
	},
	{
		prop: 'gender',
		label: '性别',
	},
	{
		prop: 'type',
		label: '用户类型',
	},
	{
		prop: 'isBlogger',
		label: '博主',
	},
	{
		prop: 'createdTime',
		label: '注册时间',
		width: 180,
	},
	{
		prop: 'action',
		label: '操作',
		fixed: 'right',
		width: 150,
		// isShow: auth('authaccount:delete'),
	},
]);

const dataCallBack = (data) => {
	data.rows.forEach((res) => {
		if (res.createdTime) {
			res.createdTime = moment(res.createdTime).format('YYYY-MM-DD');
		}
	});
	return data;
};

const onChange = async (id: string) => {
	loading.value = true;
	const { success } = await _authsService.setBlogger(id);
	loading.value = false;
	if (success) {
		tableRef.value?.reset();
	}
};

const getTableList = (params: any) => {
	let newParams = JSON.parse(JSON.stringify(params)) as AuthAccountPageQueryInput;
	return _authsService.getList(newParams);
};

// 删除角色
const onDeleteRole = async (id: string) => {
	const { success } = await _authsService.delete(id);
	if (success) {
		ElMessage.success('删除成功');
		tableRef.value?.reset();
	}
};
</script>
<style scoped></style>
