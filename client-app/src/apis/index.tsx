import AppSettings from '../configs/AppSettings';

import axiosInstance, { cancelToken } from './https';
import * as generatedApi from './searchApis';

const searchClient = new generatedApi.SearchClient(AppSettings.apiUrl, axiosInstance);
export {
  searchClient,
  cancelToken,
};
