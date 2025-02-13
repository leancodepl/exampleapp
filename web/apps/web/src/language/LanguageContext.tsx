import { createContext, useCallback, useContext, useMemo, useState } from "react"
import { IntlProvider, MessageFormatElement, useIntl } from "react-intl"
import { ReactNode } from "@tanstack/react-router"
import enMessages from "../../lang/en.json"
import plMessages from "../../lang/pl.json"

enum Languages {
  Polish = "pl",
  English = "en",
}

type LanguageContextData = {
  language: string
  setLanguage: (language: string) => void
}

const defaultLanguage = Languages.Polish

const languageContext = createContext<LanguageContextData>({ language: defaultLanguage, setLanguage: () => {} })

type EnMessages = keyof typeof enMessages
type PlMessages = keyof typeof plMessages

declare global {
  // eslint-disable-next-line @typescript-eslint/no-namespace
  namespace FormatjsIntl {
    interface Message {
      ids: EnMessages | PlMessages
    }
  }
}
type Language = (typeof Languages)[keyof typeof Languages]

const messages: Record<Language, Record<FormatjsIntl.Message["ids"], MessageFormatElement[]>> = {
  en: enMessages,
  pl: plMessages,
}

type LanguageProviderProps = {
  children?: ReactNode
}

export function LanguageProvider({ children }: LanguageProviderProps) {
  const [storedLanguage, setLanguageInternal] = useState(
    () => localStorage.getItem(languageStorageKey) ?? defaultLanguage,
  )

  const setLanguage = useCallback((language: string) => {
    localStorage.setItem(languageStorageKey, language)
    setLanguageInternal(language)
  }, [])

  const language = useMemo(
    () => (storedLanguage in messages ? (storedLanguage as Language) : defaultLanguage),
    [storedLanguage],
  )

  const value = useMemo<LanguageContextData>(() => ({ language, setLanguage }), [language, setLanguage])

  const locale = useMemo(() => {
    for (const navigatorLanguage of navigator.languages ?? [navigator.language]) {
      const navigatorLocale = new Intl.Locale(navigatorLanguage)

      if (navigatorLocale.language === language) {
        return navigatorLanguage
      }
    }

    return language
  }, [language])

  return (
    <languageContext.Provider value={value}>
      <IntlProvider defaultLocale={defaultLanguage} locale={locale} messages={messages[language]} onError={() => {}}>
        {children}
      </IntlProvider>
    </languageContext.Provider>
  )
}

const languageStorageKey = "language"

export function useLanguage() {
  const { locale } = useIntl()
  const { setLanguage } = useContext(languageContext)

  const language = useMemo(() => {
    const intlLocale = new Intl.Locale(locale)

    switch (intlLocale.language) {
      case "pl":
        return Languages.Polish
      default:
        return Languages.English
    }
  }, [locale])

  return { language, setLanguage }
}
