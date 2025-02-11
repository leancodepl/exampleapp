/// <reference types="@pigment-css/react/theme" />
import { Breakpoint, CSSObject } from "@pigment-css/react"
import { defaultDarkFilterMap } from "./utils/theme"

const breakpoints: Record<Breakpoint, number> = {
  xs: 672,
  sm: 768,
  md: 1056,
  lg: 1312,
  xl: 1548,
}

export const defaultLightColors = {
  colors: {
    background: {
      accent: {
        primary: "#414552",
        primary_hover: "#31343e",
        primary_pressed: "#292b34",
        secondary: "#979aa4",
        secondary_hover: "#858995",
        secondary_pressed: "#626776",
        tertiary: "#f7f7f8",
        tertiary_hover: "#eeeef0",
        tertiary_pressed: "#dcdde1",
      },
      active: {
        primary: "#414552",
        primary_hover: "#31343e",
        primary_pressed: "#292b34",
        secondary: "#979aa4",
        secondary_hover: "#858995",
        secondary_pressed: "#626776",
        tertiary: "#f7f7f8",
        tertiary_hover: "#eeeef0",
        tertiary_pressed: "#dcdde1",
      },
      info: {
        primary: "#1668db",
        primary_hover: "#145ec5",
        primary_pressed: "#1253af",
        secondary: "#8bb4ed",
        secondary_hover: "#5c95e6",
        secondary_pressed: "#4586e2",
        tertiary: "#e8f0fb",
        tertiary_hover: "#d0e1f8",
        tertiary_pressed: "#b9d2f4",
      },
      danger: {
        primary: "#c12c2c",
        primary_hover: "#b91e1e",
        primary_pressed: "#901717",
        secondary: "#e69090",
        secondary_hover: "#e17a7a",
        secondary_pressed: "#dc6464",
        tertiary: "#fae9e9",
        tertiary_hover: "#f5d3d3",
        tertiary_pressed: "#efb8b8",
      },
      success: {
        primary: "#0d8247",
        primary_hover: "#0c7540",
        primary_pressed: "#095b32",
        secondary: "#b6dac8",
        secondary_hover: "#9ecdb5",
        secondary_pressed: "#6eb491",
        tertiary: "#e7f3ed",
        tertiary_hover: "#cfe6da",
        tertiary_pressed: "#cfe6da",
      },
      warning: {
        primary: "#9e5200",
        primary_hover: "#8e4a00",
        primary_pressed: "#6f3900",
        secondary: "#d8ba99",
        secondary_hover: "#cfa980",
        secondary_pressed: "#c59766",
        tertiary: "#f5eee6",
        tertiary_hover: "#ecdccc",
        tertiary_pressed: "#e2cbb3",
      },
      default: {
        primary: "#ffffff",
        primary_hover: "#f9f9ff",
        primary_pressed: "#f0f2fa",
        secondary: "#f9f9ff",
        secondary_hover: "#f0f2fa",
        secondary_pressed: "#e1e1eb",
        tertiary: "#e1e1eb",
        tertiary_hover: "#cecedb",
        tertiary_pressed: "#a3a3b3",
        scrim: "#040e2933",
      },
      inverse: {
        primary: "#18181d",
        primary_hover: "#2f2f39",
        primary_pressed: "#3f3f4d",
      },
      disabled: {
        primary: "#e1e1eb",
        secondary: "#f0f2fa",
        tertiary: "#f9f9ff",
      },
    },
    foreground: {
      accent: {
        primary: "#414552",
        primary_hover: "#31343e",
        primary_pressed: "#292b34",
        secondary: "#515667",
        secondary_hover: "#414552",
        secondary_pressed: "#31343e",
        tertiary: "#626776",
        quaternary: "#cbccd1",
      },
      active: {
        primary: "#414552",
        primary_hover: "#31343e",
        primary_pressed: "#292b34",
        secondary: "#515667",
        secondary_hover: "#414552",
        secondary_pressed: "#31343e",
        tertiary: "#626776",
        quaternary: "#cbccd1",
      },
      info: {
        primary: "#1668db",
        primary_hover: "#145ec5",
        primary_pressed: "#1253af",
        secondary: "#4586e2",
        secondary_hover: "#2d77df",
        secondary_pressed: "#1668db",
        tertiary: "#8bb4ed",
        quaternary: "#b9d2f4",
      },
      danger: {
        primary: "#c12c2c",
        primary_hover: "#b91e1e",
        primary_pressed: "#901717",
        secondary: "#dc6464",
        secondary_hover: "#d44343",
        secondary_pressed: "#c12c2c",
        tertiary: "#e69090",
        quaternary: "#efb8b8",
      },
      success: {
        primary: "#0d8247",
        primary_hover: "#0c7540",
        primary_pressed: "#095b32",
        secondary: "#6eb491",
        secondary_hover: "#3d9b6c",
        secondary_pressed: "#0d8247",
        tertiary: "#b6dac8",
        quaternary: "#cfe6da",
      },
      warning: {
        primary: "#9e5200",
        primary_hover: "#8e4a00",
        primary_pressed: "#6f3900",
        secondary: "#c59766",
        secondary_hover: "#b17533",
        secondary_pressed: "#9e5200",
        tertiary: "#d8ba99",
        quaternary: "#e2cbb3",
      },
      default: {
        primary: "#18181d",
        primary_hover: "#282830",
        primary_pressed: "#2f2f39",
        secondary: "#606071",
        secondary_hover: "#4f4f60",
        secondary_pressed: "#3f3f4d",
        tertiary: "#808093",
        quaternary: "#cecedb",
      },
      inverse: {
        primary: "#f9f9ff",
        primary_hover: "#f0f2fa",
        primary_pressed: "#e1e1eb",
      },
      disabled: {
        primary: "#808093",
        secondary: "#a3a3b3",
        tertiary: "#cecedb",
      },
    },
  },
  shadows: {
    xs: "0 1px 2px 0 #F5F5FA",
    sm: "0 1px 3px 0 #F5F5FA",
    md: "0px 0px 0px 3px #F5F5FA",
    lg: "0 10px 15px -3px #F5F5FA",
    error: "0px 0px 0px 3px #FDE8E8",
  },
}

export const defaultDarkColors: typeof defaultLightColors = JSON.parse(JSON.stringify(defaultLightColors), (k, v) =>
  typeof v === "string" ? defaultDarkFilterMap(v) : v,
)

export const defaultTheme = {
  radii: {
    xs: "0.125rem",
    sm: "0.25rem",
    md: "0.375rem",
    lg: "0.5rem",
    xl: "0.75rem",
    xl2: "1rem",
    xl3: "1.5rem",
    xl4: "2rem",
    full: "9999px",
  },

  spacing: {
    _0: "0",
    _1: "0.25rem",
    _2: "0.5rem",
    _3: "0.75rem",
    _4: "1rem",
    _5: "1.25rem",
    _6: "1.5rem",
    _8: "2rem",
    _12: "3rem",
  },
  breakpoints: {
    ...breakpoints,
    up: Object.fromEntries(
      Object.entries(breakpoints).map(([breakpoint, value]) => {
        return [breakpoint, `@media (min-width: ${value}px)`]
      }),
    ) as Record<keyof typeof breakpoints, string>,
    down: Object.fromEntries(
      Object.entries(breakpoints).map(([breakpoint, value]) => {
        return [breakpoint, `@media (max-width: ${value - 0.1}px)`]
      }),
    ) as Record<keyof typeof breakpoints, string>,
  },
}

export type ThemeTokens = typeof defaultLightColors & typeof defaultTheme

export type Theme = {
  colorScheme: "dark" | "light"
  tokens: ThemeTokens
}

declare module "@pigment-css/react/theme" {
  interface ThemeArgs {
    theme: StrictlyExtendTheme<Theme>
  }
}

type StrictlyExtendTheme<
  Options extends {
    colorScheme: string
    tokens: Record<string, any>
  } = {
    colorScheme: string
    tokens: Record<string, any>
  },
> = {
  vars: Options["tokens"]
  applyStyles: (colorScheme: Options["colorScheme"], styles: CSSObject<any>) => Record<string, CSSObject<any>>
  getColorSchemeSelector: (colorScheme: Options["colorScheme"]) => string
  generateStyleSheets: () => Array<Record<string, any>>
} & Options["tokens"]
