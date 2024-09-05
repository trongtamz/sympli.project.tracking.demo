import { configureStore } from '@reduxjs/toolkit';
import languageReducer from './languageSlice';
import searchReducer from './searchSlice';
const store = configureStore({
  reducer: {
    search: searchReducer,
    language: languageReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
