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
import { ExceptionlogsServiceProxy } from '@/shared/service-proxies';
import moment from 'moment';

const _exceptionlogService = new ExceptionlogsServiceProxy(inject('$baseurl'), inject('$api'));

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
    prop: 'clientIP',
    label: '客户端IP',
    width: 150,
  },
  {
    prop: 'message',
    label: '异常信息',
    width: 300,
  },
  {
    prop: 'source',
    label: '异常来源',
    width: 150,
  },
  {
    prop: 'stackTrace',
    label: '异常堆栈信息',
    width: 300,
    // isShow: auths(['category:edit', 'category:delete']),
  },
  {
    prop: 'type',
    label: '异常类型',
    width: 150,
  },
  {
    prop: 'operatorName',
    label: '操作人',
    width: 150,
  },
  {
    prop: 'userOS',
    label: '操作系统',
    width: 150,
  },
  {
    prop: 'userAgent',
    label: '用户代理',
    width: 150,
  },
  {
    prop: 'creationTime',
    label: '创建时间',
    width: 180,
  },
  {
    prop: 'action',
    label: '操作',
    width: 150,
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
  console.log(params);
  let newParams = JSON.parse(JSON.stringify(params));
  return await _exceptionlogService.getPage(newParams);
};

//删除机构
const onDelete = async (id: string) => {
  const { success } = await _exceptionlogService.delete(id);
  if (success) {
    ElMessage.success('删除成功');
    tableRef.value?.reset();
  }
};
</script>
