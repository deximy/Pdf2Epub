import * as tus from "tus-js-client";
import Emitter from "tiny-emitter";

let emitter = new Emitter.TinyEmitter();

const api_url = "https://localhost:49153";

const UploadFile = async (
    file: File,
    location: string,
    OnProcess: (percent: number) => void,
    OnFinish: (url: string | null) => void,
    OnError: () => void
) => {
    let tus_client = new tus.Upload(
        file,
        {
            endpoint: `${api_url}/upload`,
            retryDelays: [0, 3000, 5000, 10000, 20000],
            metadata: {
                name: file.name,
                contentType: file.type || "application/octet-stream",
                emptyMetaKey: "",
                location: location,
            },
            onError: (error) => {
                console.log("Failed because: " + error);
                OnError();
            },
            onProgress: (bytesUploaded, bytesTotal) => {
                let percent = (bytesUploaded / bytesTotal * 100);
                OnProcess(percent);
            },
            onSuccess: () => {
                console.log("Succeeded. Download %s from %s", (tus_client.file as File).name, tus_client.url);
                OnFinish(tus_client.url);
            }
        }
    );
    tus_client
        .findPreviousUploads()
        .then(
            (previousUploads) => {
                // Found previous uploads so we select the first one.
                if (previousUploads.length) {
                    tus_client.resumeFromPreviousUpload(previousUploads[0]);
                }

                // Start the upload
                tus_client.start();
            }
        );
};

const GetEmitter = () => {
    return emitter;
}

const ConvertPdf = async (file_name: string, is_vertical: boolean) => {
    let form_data = new FormData();
    form_data.set("file_id", file_name);
    form_data.set("is_vertical", is_vertical ? "true" : "false");
    let response = await fetch(
        `${api_url}/convert`,
        {
            method: "POST",
            body: form_data
        }
    );
    return await response.text();
};

const QueryConvertProgress = async (task_id: string) => {
    let response = await fetch(
        `${api_url}/convert/${task_id}?type=progress`,
        {
            method: "Get",
        }
    );
    return await response.text();
};

const GetConvertResult = async (task_id: string) => {
    let response = await fetch(
        `${api_url}/convert/${task_id}?type=result`,
        {
            method: "Get",
        }
    );
    return await response.text();
};

export {GetEmitter, UploadFile};
export {ConvertPdf, QueryConvertProgress, GetConvertResult};