<script lang="ts" setup>
import { NUpload
       , NUploadDragger
       , UploadCustomRequestOptions
       , UploadFileInfo
       } from "naive-ui/es";
import {NIcon} from "naive-ui/es";
import {ArchiveOutline as ArchiveIcon} from "@vicons/ionicons5";
import {NText, NP} from "naive-ui/es";

import {ref} from "vue";
import * as path from "path-browserify";
import {UploadFile} from "../apis/api";
import {GetEmitter} from "../apis/api";
import {ConvertPdf} from "../apis/api";

const emitter = GetEmitter();
emitter.on(
    "ConvertAllFile",
    () => {
        if (file_list.value === undefined)
        {
            return;
        }

        for (let i of file_list.value)
        {
            if (i.status == "finished")
            {
                ConvertPdf(
                    i.id,
                    false
                ).then(
                    task_id => {
                        emitter.emit("UpdateConvertFileList", task_id, i.name);
                    }
                );
            }
        }
    }
);

const file_list = ref<Array<UploadFileInfo>>();     // fxxk, why is there a type error here
const CustomUploadFile = (
    {
        file: file_info,
        onProgress: OnProgess,
        onFinish: OnFinish,
        onError: OnError,
    }: UploadCustomRequestOptions
) => {
    let file = file_info.file as File;
    if (file.webkitRelativePath != "" && file.webkitRelativePath != undefined)
    {
        // upload directory
        let target_path = file.webkitRelativePath.substring(0, file.webkitRelativePath.lastIndexOf('/'));
        // let full_path = path.join(current_path.value, target_path);
        // CreateDirectory(full_path)
        //     .then(
        //         () => {
        //             UploadFile(file, full_path, onFinish, onError).then(() => RefreshDirectory(current_path.value));
        //         }
        //     );
    }
    else
    {
        // upload file(s)
        UploadFile(
            file,
            file.name,
            (percent: number) => {
                OnProgess({percent});
            },
            (url: string | null) => {
                OnFinish();

                if (file_list.value === undefined)
                {
                    return;
                }

                for (let i of file_list.value)
                {
                    if (i.id === file_info.id)
                    {
                        i.url = url;
                    }
                }

                emitter.emit("OnFileUpload");
            },
            OnError
        );
    }
};
</script>

<template>
    <div class="uploader">
        <n-upload
            v-model:file-list="file_list"
            :multiple="true"
            :custom-request="CustomUploadFile"
        >
            <n-upload-dragger>
                <div style="margin-bottom: 12px">
                    <n-icon size="48" :depth="3">
                        <archive-icon />
                    </n-icon>
                </div>
                <n-text style="font-size: 16px">
                    点击或者拖动文件到该区域来上传
                </n-text>
                <n-p depth="3" style="margin: 8px 0 0 0">
                    请不要上传敏感数据，比如你的银行卡号和密码，信用卡号有效期和安全码
                </n-p>
            </n-upload-dragger>
        </n-upload>
    </div>
</template>

<style scoped>
>>> .n-upload-trigger
{
    margin-left: calc((100% - 512px) / 2);
}
</style>