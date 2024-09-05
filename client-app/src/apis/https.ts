import axios, { AxiosRequestConfig } from 'axios';

import i18next from 'i18next';
import { showNotification } from '../common/utils/toastNotification';

const axiosInstance = axios.create({
  timeout: 180000,
  headers: {
    'content-type': 'application/json',
  },
  transformResponse: (data) => data,
});

declare module 'axios' {
  export interface AxiosInstance {
    ignoreError: boolean;
  }
}

export const statusCodes = {
  status503ServiceUnavailable: 503,
  status400BadRequest: 401,
  status401Unauthorized: 401,
  status403Forbidden: 403,
  status500InternalError: 500,
};

const requestInterceptor = (config: AxiosRequestConfig) => {
  // Add your request interceptors here (authorization)
  return config;
};

axiosInstance.interceptors.request.use(requestInterceptor as any, (error) => error);

axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (axios.isCancel(error)) {
      return new Promise(() => {});
    }
    const { config, data, status } = error.response;

    if (
      error.request.responseType === 'blob' &&
      data instanceof Blob &&
      data.type &&
      data.type.toLowerCase().indexOf('json') !== -1
    ) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onload = () => {
          resolve(Promise.reject(error));
        };

        reader.onerror = () => {
          reject(error);
        };

        reader.readAsText(data);
      });
    }

    if (axiosInstance.ignoreError) {
      return Promise.reject(error);
    }

    await handleErrorStatusCode(status, data);

    return Promise.reject(error);
  }
);

const handleErrorStatusCode = async (status: any, data: string) => {
  switch (status) {
    case statusCodes.status400BadRequest:
      showNotification({
        message: i18next.t('message.badRequest'),
        severity: 'error',
        autoHideDuration: 3000,
      });
      break;
    case statusCodes.status503ServiceUnavailable:
      showNotification({
        message: i18next.t('message.serviceUnavailable'),
        severity: 'error',
        autoHideDuration: 3000,
      });
      window.setTimeout(() => {
        window.location.href = '/';
      }, 6000);
      break;
    case statusCodes.status500InternalError:
      showNotification({
        message: i18next.t('message.internalError'),
        severity: 'error',
        autoHideDuration: 3000,
      });
      break;
    default:
      const responseDataObject = JSON.parse(data);
      showNotification({
        message: responseDataObject?.detail || responseDataObject?.title || i18next.t('message.unexpectedError'),
        severity: 'error',
        autoHideDuration: 5000,
      });
      break;
  }
};

export const cancelToken = axios.CancelToken;

export default axiosInstance;
