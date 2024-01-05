<template>
  <div class="blog-category-container layout-padding">
    <ProTable ref="tableRef" :request-api="getTableList" :data-callback="dataCallBack" :pagination="true"
      :columns="columns" :tool-button="false">
      <!-- v-auth="'category:edit'"  -->
      <template #action="scope">
        <el-button icon="ele-Pointer" size="small" text type="primary" @click="onOpenCategory(scope.row)"> 查看 </el-button>
        <el-popconfirm title="确认删除吗？" @confirm="onDelete(scope.row.id)">
          <template #reference>
            <!-- v-auth="'category:delete'" -->
            <el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
          </template>
        </el-popconfirm>
      </template>
      <template #fileSize="scope">
        <span>{{ (scope.row.fileSize / 1024).toFixed(2) }}KB</span>
      </template>
      <template #isFolder="scope">
        <el-tag :type="scope.row.isFolder ? 'success' : 'danger'"> {{ scope.row.isFolder ? '是' : '否' }}</el-tag>
      </template>
    </ProTable>
    <ImgDialog ref="imgDialogRef"/>
    <video></video>
  </div>
</template>

<script setup lang="ts" name="fileInfo">
import { ref, reactive, inject, defineAsyncComponent } from 'vue';
import { ElMessage } from 'element-plus';

// 引入组件
import ProTable from '@/components/ProTable/index.vue';
import type { ColumnProps } from '@/components/ProTable/interface';
import moment from 'moment';
import { FileInfoOutput, FileType, FilesServiceProxy, KeyDto } from '@/shared/service-proxies';
const ImgDialog = defineAsyncComponent(() => import('@/views/system/fileinfo/dialog.vue'));

const _fileService = new FilesServiceProxy(inject('$baseurl'), inject('$api'));

const tableRef = ref<InstanceType<typeof ProTable>>();

  const imgDialogRef = ref<InstanceType<typeof ImgDialog>>();

const columns = reactive<ColumnProps[]>([
  {
    prop: 'fileDisplayName',
    label: '显示名称',
    search: { el: 'input', key: 'name', label: '搜索条件' },
    align: 'left',
    width: 300,
  },
  {
    prop: 'fileExt',
    label: '文件扩展名',
    width: 150,
  },
  {
    prop: 'contentType',
    label: '文件类型',
    width: 300,
  },
  {
    prop: 'filePath',
    label: '文件路径',
    width: 300,
    // isShow: auths(['category:edit', 'category:delete']),
  },
  {
    prop: 'fileSize',
    label: '文件大小，字节',
    width: 150,
  },
  {
    prop: 'fileTypeString',
    label: '文件类型',
    width: 150,
  },
  {
    prop: 'code',
    label: '编码',
    width: 150,
  },
  {
    prop: 'isFolder',
    label: '是否是文件夹',
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
  console.log(params);
  let newParams = JSON.parse(JSON.stringify(params));
  return await _fileService.getPage(newParams);
};

//删除机构
const onDelete = async (id: string) => {
  const { success } = await _fileService.delete({ id: id } as KeyDto);
  if (success) {
    ElMessage.success('删除成功');
    tableRef.value?.reset();
  }
};

const onOpenCategory = async (row: FileInfoOutput) => {
  console.log(row);
  if(row.fileType === FileType._2){
    location.href = `${row.fileIpAddress}/${row.filePath}`;
    return;
  } 
  imgDialogRef.value.openDialog(row);
};
</script>
