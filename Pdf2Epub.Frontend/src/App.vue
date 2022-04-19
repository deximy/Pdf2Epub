<script setup lang="ts">
import Uploader from "./components/Uploader.vue";
import Converter from "./components/Converter.vue";
import { NGrid
       , NGridItem
       , NTabs
       , NTabPane
       , NCard
       , NDropdown
       , NSwitch
       , NText
       , NSpace
       } from "naive-ui/es";
import {NButton} from "naive-ui/es";

import {ref, h} from "vue";
import {GetEmitter} from "./apis/api";

const convert_disabled = ref(true);
const is_vertical = ref(false);

const emitter = GetEmitter();
emitter.on(
    "OnFileUpload",
    () => {
        convert_disabled.value = false;
    }
);

const StartConvertAllFile = () => {
    emitter.emit("ConvertAllFile");
};

const renderCustomHeader = () => {
  return h(
    'div',
    {
        style: 'display: flex; align-items: center; padding: 8px 12px;',
    },
    [
        h(NSpace, [
            h(NText, { depth: 3, }, { default: () => '竖直排版', }),
            h(NSwitch, {
                'on-update:value': (value: boolean) => {
                    is_vertical.value = value;
                }
            }),
        ]),
    ]
  )
}

const advanced_options = [
    {
        key: "header",
        type: "render",
        render: renderCustomHeader,
    },
]
</script>

<template>
    <div class="app">
        <n-tabs type="line" animated>
            <template #prefix>a logo goes here</template>
            <n-tab-pane name="converter" tab="转换器">
                <n-card>
                    <uploader />
                    <div class="convert-wrapper">
                        <n-button
                            type="primary"
                            class="convert-all"
                            :disabled="convert_disabled"
                            @click="StartConvertAllFile"
                        >
                            一键转换全部文件
                        </n-button>
                        <n-dropdown trigger="click" :options="advanced_options" @select="handleAdvancedSelect">
                            <n-button
                                type="primary"
                                secondary
                                class="convert-with-more-setting"
                                :disabled="convert_disabled"
                            >
                                高级设置
                            </n-button>
                        </n-dropdown>
                    </div>
                    <converter />
                </n-card>
            </n-tab-pane>
            <n-tab-pane name="features" tab="特性">
                <n-grid :x-gap="12" :y-gap="8" :cols="3">
                    <n-grid-item>
                        <n-card title="各类版式">
                            横排、竖排、双栏……
                        </n-card>
                    </n-grid-item>
                    <n-grid-item>
                        <n-card title="语言适配">
                            用合适的方式分别处理中文、西文文章
                        </n-card>
                    </n-grid-item>
                    <n-grid-item>
                        <n-card title="保留样式">
                            保留原文档文字的原有样式
                        </n-card>
                    </n-grid-item>
                    <n-grid-item>
                        <n-card title="正确分段">
                            根据文章内容将文字行汇聚为自然段
                        </n-card>
                    </n-grid-item>
                    <n-grid-item>
                        <n-card title="无损转换">
                            保留原文章标题、作者、目录等信息，使其不丢失
                        </n-card>
                    </n-grid-item>
                    <n-grid-item>
                        <n-card title="高效转换">
                            速度快（存疑）
                        </n-card>
                    </n-grid-item>
                </n-grid>
            </n-tab-pane>
            <n-tab-pane name="about" tab="关于">
                <p>团队信息：團団团（2106280）</p>
                <p>团队成员：戴宇轩、梁天宇、高珏霖</p>
            </n-tab-pane>
        </n-tabs>
    </div>
</template>

<style scoped>
.app {
    padding-left: 10px;
    padding-right: 10px;
}
.light-green {
  height: 108px;
  background-color: rgba(0, 128, 0, 0.12);
}
.green {
  height: 108px;
  background-color: rgba(0, 128, 0, 0.24);
}
.convert-wrapper
{
    display: flex;
    justify-content: space-around;

    margin-top: 50px;
}
.converter
{
    margin-top: 50px;
}
</style>
