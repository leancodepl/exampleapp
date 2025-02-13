import { css, styled } from "@pigment-css/react"
import { breakpoints } from "../.."

export const MediaContainer = styled.div`
  display: contents;
`

export const mediaFromMap = {
  xs: css`
    ${breakpoints.down.xs} {
      display: none;
    }
  `,
  sm: css`
    ${breakpoints.down.sm} {
      display: none;
    }
  `,
  md: css`
    ${breakpoints.down.md} {
      display: none;
    }
  `,
  lg: css`
    ${breakpoints.down.lg} {
      display: none;
    }
  `,
  xl: css`
    ${breakpoints.down.xl} {
      display: none;
    }
  `,
}

export const mediaToMap = {
  xs: css`
    ${breakpoints.up.xs} {
      display: none;
    }
  `,
  sm: css`
    ${breakpoints.up.sm} {
      display: none;
    }
  `,
  md: css`
    ${breakpoints.up.md} {
      display: none;
    }
  `,
  lg: css`
    ${breakpoints.up.lg} {
      display: none;
    }
  `,
  xl: css`
    ${breakpoints.up.xl} {
      display: none;
    }
  `,
}

export const themeMap = {
  light: css`
    [data-theme="dark"] & {
      display: none;
    }
  `,
  dark: css`
    [data-theme="light"] & {
      display: none;
    }
  `,
}
