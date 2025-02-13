import { createContext, useCallback, useContext, useEffect, useMemo, useState } from "react"
import { ReactNode } from "@tanstack/react-router"

type Theme = "dark" | "light" | "system"

type ThemeContextData = {
  theme: Theme
  setTheme: (theme: Theme) => void
}

const themeContext = createContext<ThemeContextData>({ theme: "system", setTheme: () => {} })

type ThemeProviderProps = {
  children?: ReactNode
}

export function ThemeProvider({ children }: ThemeProviderProps) {
  const [theme, setTheme_] = useState<Theme>(() => getTheme(localStorage.getItem(localStorageThemeKey)))

  useEffect(() => {
    const themeToApply = (() => {
      if (theme === "system") return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light"

      return theme
    })()

    document.documentElement.dataset.theme = themeToApply
  }, [theme])

  useEffect(() => {
    const handler = (ev: StorageEvent) => {
      if (ev.storageArea === localStorage && ev.key === localStorageThemeKey) {
        setTheme_(getTheme(ev.newValue))
      }
    }

    window.addEventListener("storage", handler)

    return () => window.removeEventListener("storage", handler)
  }, [])

  const setTheme = useCallback((theme: Theme) => {
    setTheme_(theme)
    localStorage.setItem(localStorageThemeKey, theme)
  }, [])

  const value = useMemo<ThemeContextData>(() => ({ theme, setTheme }), [setTheme, theme])

  return <themeContext.Provider value={value}>{children}</themeContext.Provider>
}

const localStorageThemeKey = "theme"

function getTheme(theme?: string | null): Theme {
  switch (theme) {
    case "dark":
      return "dark"
    case "light":
      return "light"
    default:
      return "system"
  }
}

export function useTheme() {
  return useContext(themeContext)
}
