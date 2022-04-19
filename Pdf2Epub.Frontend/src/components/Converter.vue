<script lang="ts" setup>
import {NUpload, NUploadFileList, UploadFileInfo} from "naive-ui";

import {ref} from "vue";
import {GetEmitter} from "../apis/api";
import {QueryConvertProgress, GetConvertResult} from "../apis/api";

const file_list = ref<Array<UploadFileInfo>>();
const emitter = GetEmitter();
emitter.on(
    "UpdateConvertFileList",
    (task_id: string, file_name: string) => {
        file_list.value?.push(
            {
                id: task_id,
                name: `${file_name}.pdf`,
                status: "uploading",
                type: "text/plain",
                percentage: 0
            }
        );
        const UpdateQueryProgessLoop = () => {
            setTimeout(
                () => {
                    QueryConvertProgress(
                        task_id
                    ).then(
                        (result) => {
                            if (file_list.value === undefined)
                            {
                                return;
                            }

                            for (let i in file_list.value)
                            {
                                if (file_list.value[i].id == task_id)
                                {
                                    file_list.value[i].percentage = parseInt(result);
                                }

                                if (file_list.value[i].percentage == 100)
                                {
                                    file_list.value[i].status = "finished";
                                    GetConvertResult(task_id).then(value => file_list.value[i].url = value);
                                }
                            }

                            if (parseInt(result) != 100)
                            {
                                UpdateQueryProgessLoop();
                            }
                        }
                    );
                },
                1000
            );
        };
        UpdateQueryProgessLoop();
    }
);
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