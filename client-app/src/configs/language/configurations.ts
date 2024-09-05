import enResources from "../locales/en";


const langObject = {
  code: 'en',
  ns: 'common',
  languages: {
    en: 'en',
  },
};
const langResources = {
  [langObject.languages.en]: enResources,
};

export const appLang = langObject;
export const appLangResources = langResources;
