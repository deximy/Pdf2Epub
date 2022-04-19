<script lang="ts" setup>
import {NUpload, NUploadFileList} from "naive-ui";
import {ref} from "vue";
import {UploadFileInfo} from "naive-ui/es";

const file_list = ref<Array<UploadFileInfo>>();

file_list.value = [

];
</script>

<template>
    <div class="converter">
        <n-upload
            abstract
            :file-list="file_list"
        >
            <n-upload-file-list />
        </n-upload>
    </div>
</template>

<style scoped>

</style>