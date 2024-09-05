import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { SearchEngineEnum } from '../apis/searchApis';

interface SearchFields {
  keywords: string;
  url: string;
  engine: SearchEngineEnum;
}

interface SearchState {
  fields: SearchFields;
  results: string;
}

const initialState: SearchState = {
  fields: {
    keywords: '',
    url: '',
    engine: SearchEngineEnum.Google,
  },
  results: '',
};

const searchSlice = createSlice({
  name: 'search',
  initialState,
  reducers: {
    setFields: (state, action: PayloadAction<{ field: keyof SearchFields; value: string }>) => {
      const { field, value } = action.payload;
      state.fields[field] = value as any;
    },
    setResults: (state, action: PayloadAction<string>) => {
      state.results = action.payload;
    },
  },
});

export const { setFields, setResults } = searchSlice.actions;
export default searchSlice.reducer;
