<template>
  <div>
    <el-dialog :title="state.dialog.title" v-model="state.dialog.isShowDialog" :destroy-on-close="true" width="60%">
      <div v-if="state.dialog.fileType === FileType._1">
        <video controls style="width: 100%;" :loop="true" preload="auto">
          <!-- MP4 文件 -->
          <source :src="state.dialog.playvideo" type="video/mp4" />
          <source :src="state.dialog.playvideo" type="video/webm" />
          <source :src="state.dialog.playvideo" type="video/ogg" />
        </video>
      </div>
      <div v-if="state.dialog.fileType === FileType._0">
        <el-image style="width: 100%;" ref="previewImg" :preview-src-list="[state.dialog.playvideo]"
          :src="state.dialog.playvideo">
        </el-image>
      </div>

      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel" size="default">取 消</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts" name="imgDialog">
import { FileInfoOutput, FileType } from '@/shared/service-proxies';
import { reactive } from 'vue';


//表单数据
const state = reactive({
  dialog: {
    isShowDialog: false,
    type: '',
    title: '',
    submitTxt: '',
    loading: true,
    playvideo: '',
    fileType: FileType._0
  },
});

// 打开弹窗
const openDialog = async (row: FileInfoOutput) => {
  state.dialog.isShowDialog = true;
  state.dialog.playvideo = `${row.fileIpAddress}/${row.filePath}`;
  state.dialog.fileType = row.fileType;
};
// 关闭弹窗
const closeDialog = () => {
  state.dialog.isShowDialog = false;
};
// 取消
const onCancel = () => {
  closeDialog();
};
// 暴露变量
defineExpose({
  openDialog,
});
</script>
<style scoped lang="scss"></style>
