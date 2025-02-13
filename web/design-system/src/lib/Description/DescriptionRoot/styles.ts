import { css, styled } from "@pigment-css/react"
import { breakpoints, spacing } from "../../../utils"

export const StyledDescriptionRoot = styled("div", { name: "Description", slot: "Root" })`
  display: grid;
  grid-template-columns: repeat(var(--description-root-columns), 1fr);
  gap: ${spacing._4};
`

export const descriptionBreakpointMap = {
  xs: css`
    ${breakpoints.up.xs} {
      grid-template-columns: repeat(var(--description-root-columns-xs), 1fr);
    }
  `,
  sm: css`
    ${breakpoints.up.sm} {
      grid-template-columns: repeat(var(--description-root-columns-sm), 1fr);
    }
  `,
  md: css`
    ${breakpoints.up.md} {
      grid-template-columns: repeat(var(--description-root-columns-md), 1fr);
    }
  `,
  lg: css`
    ${breakpoints.up.lg} {
      grid-template-columns: repeat(var(--description-root-columns-lg), 1fr);
    }
  `,
  xl: css`
    ${breakpoints.up.xl} {
      grid-template-columns: repeat(var(--description-root-columns-xl), 1fr);
    }
  `,
}
