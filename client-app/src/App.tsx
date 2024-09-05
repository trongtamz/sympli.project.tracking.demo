import { LoadingButton } from '@mui/lab';
import {
  Box,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
  TextField,
  Typography,
} from '@mui/material';
import { t } from 'i18next';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from 'store';
import { searchClient } from './apis';
import { SearchEngineEnum } from './apis/searchApis';
import './App.css';
import reactLogo from './assets/react.svg';
import { setFields, setResults } from './store/searchSlice';
import viteLogo from '/vite.svg';
function App() {
  const [onLoading, setOnLoading] = useState(false);
  const dispatch = useDispatch();
  const { fields, results } = useSelector((state: RootState) => state.search);

  const defaultKeywords = 'e-settlements';
  const defaultUrl = 'https://www.sympli.com.au';

  useEffect(() => {
    dispatch(setFields({ field: 'keywords', value: defaultKeywords }));
    dispatch(setFields({ field: 'url', value: defaultUrl }));
  }, [dispatch]);

  const handleTextChange = (field: keyof typeof fields) => (e: React.ChangeEvent<HTMLInputElement>) => {
    dispatch(setFields({ field, value: e.target.value }));
  };

  const handleSelectChange = (e: SelectChangeEvent<SearchEngineEnum>) => {
    const value = e.target.value as SearchEngineEnum;
    dispatch(setFields({ field: 'engine', value }));
  };
  // const engines = [SearchEngineEnum.Google, SearchEngineEnum.Bing];

  // const onChangeKeywords = (value: string) => {
  //   setRequest({ ...request, keywords: value });
  // };
  // const onChangeUrl = (value: string) => {
  //   setRequest({ ...request, url: value });
  // };
  // const onChangeEngine = (event: SelectChangeEvent) => {
  //   setRequest({ ...request, searchEngine: event.target.value as SearchEngineEnum });
  // };
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    dispatch(setResults(''));
    setOnLoading(true);
    try {
      const result = await searchClient.search(fields.keywords, fields.url, fields.engine);
      if (result && result.Data) {
        dispatch(setResults(result.Data));
      }
    } catch {}
    setOnLoading(false);
  };
  // const onCount = async () => {
  //   setResult('');
  //   setOnLoading(true);
  //   try {
  //     const result = await searchClient.search(request.keywords, request.url, request.searchEngine);
  //     if (result && result.Data) {
  //       setResult(result.Data);
  //     }
  //   } catch {}
  //   setOnLoading(false);
  // };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{ display: 'flex', flexDirection: 'column', gap: 2, maxWidth: 400, margin: '0 auto' }}
    >
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <Box sx={{ display: 'flex', flexDirection: 'column' }}>
        <TextField
          placeholder={defaultKeywords}
          label={t('keywords')}
          variant="outlined"
          value={fields.keywords}
          onChange={handleTextChange('keywords')}
          sx={{ mb: 2 }}
        />
        <TextField
          placeholder={defaultUrl}
          label={t('url')}
          variant="outlined"
          value={fields.url}
          defaultValue={t('defaultUrl')}
          onChange={handleTextChange('url')}
          sx={{ mb: 2 }}
        />
        <FormControl size="medium" variant="outlined">
          <InputLabel id="engine-label">{t('engine')}</InputLabel>
          <Select
            labelId="engine-label"
            id="demo-simple-select-autowidth"
            value={fields.engine}
            onChange={handleSelectChange}
            label={t('engine')}
          >
            {Object.values(SearchEngineEnum).map((option) => (
              <MenuItem key={option} value={option}>
                {option}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>

      <div className="card">
        <LoadingButton loading={onLoading} type="submit" variant="contained" color="primary">
          {t('count')}
        </LoadingButton>
      </div>
      <Typography variant="h6">{results}</Typography>
    </Box>
  );
}

export default App;

