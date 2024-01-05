<template>
    <div class="blog-category-container layout-padding">
        <ProTable ref="tableRef" :request-api="getTableList" :data-callback="dataCallBack" :pagination="true"
            :columns="columns" :tool-button="false">
            <!-- v-auth="'category:edit'"  -->
            <template #action="scope">
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
import { ref, reactive, inject } from 'vue';
import { ElMessage } from 'element-plus';

// 引入组件
import ProTable from '@/components/ProTable/index.vue';
import type { ColumnProps } from '@/components/ProTable/interface';
import { RequestLogsServiceProxy } from '@/shared/service-proxies';
import moment from 'moment';

const _requestLogsService = new RequestLogsServiceProxy(inject('$baseurl'), inject('$api'));

const tableRef = ref<InstanceType<typeof ProTable>>();

const columns = reactive<ColumnProps[]>([
    {
        prop: 'requestUri',
        label: '请求URI',
        search: { el: 'input', key: 'name', label: '搜索条件' },
        align: 'left',
        width: 300,
    },
    {
        prop: 'requestType',
        label: '请求方式',
        width: 150,
    },
    {
        prop: 'requestData',
        label: '请求数据',
        width: 200,
    },
    {
        prop: 'responseData',
        label: '响应数据',
        width: 300,
    },
    {
        prop: 'userName',
        label: '用户姓名',
        width: 150,
        // isShow: auths(['category:edit', 'category:delete']),
    },
    {
        prop: 'clientIP',
        label: '访问ip',
        width: 300,
    },
    {
        prop: 'userAgent',
        label: '用户代理',
        width: 150,
    },
    {
        prop: 'userOS',
        label: '操作系统',
        width: 150,
    },
    {
        prop: 'spendTime',
        label: '耗时',
        width: 150,
    },
    {
        prop: 'creationTime',
        label: '创建时间',
        width: 300,
    },
    {
        prop: 'action',
        label: '操作',
        width: 180,
        fixed: 'right',
    },
]);

const dataCallBack = (data) => {
    data.rows.forEach((res) => {
        if (res.creationTime) {
            res.creationTime = moment(res.creationTime).format('yyyy-MM-DD HH:mm:ss');
        }
    });
    return data;
};

const getTableList = async (params: any) => {
    let newParams = JSON.parse(JSON.stringify(params));
    return await _requestLogsService.getPage(newParams);
};

//删除机构
const onDelete = async (id: string) => {
    const { success } = await _requestLogsService.delete(id);
    if (success) {
        ElMessage.success('删除成功');
        tableRef.value?.reset();
    }
};
</script>
