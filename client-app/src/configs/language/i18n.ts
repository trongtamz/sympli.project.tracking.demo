import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import Backend from 'i18next-http-backend';
import { initReactI18next } from 'react-i18next';

import { appLang, appLangResources } from './configurations';

i18next
  .use(Backend)
  .use(LanguageDetector)
  .use(initReactI18next)
  .init(
    {
      ns: appLang.ns,
      defaultNS: appLang.ns,
      interpolation: { escapeValue: false },
      react: { useSuspense: true },
      lng: appLang.code,
      fallbackLng: appLang.code,
      resources: appLangResources,
    },
    (err: any) => {
      if (err) console.log('I18n ERROR:', err);
    }
  );

export default i18next;
