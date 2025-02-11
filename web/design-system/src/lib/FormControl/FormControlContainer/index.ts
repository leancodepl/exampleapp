import { styled } from "@pigment-css/react"
import { colors, radii, shadows, spacing } from "../../../utils"
import { dataError, dataSize } from "../attributes"

export const FormControlContainer = styled("div", { name: "FormControl", slot: "container" })`
  display: flex;
  flex: 0 0 auto;
  gap: ${spacing._2};
  align-items: center;
  height: 2.5rem;

  background-color: inherit;
  border: 1px solid;
  border-color: ${colors.foreground.default.tertiary};
  border-radius: ${radii.md};
  padding-inline: ${spacing._3};

  svg {
    color: ${colors.foreground.default.secondary};
  }

  &:focus-within {
    border-color: ${colors.foreground.accent.primary};
    box-shadow: ${shadows.md};
  }

  &:focus-visible {
    outline: none;
  }

  ${dataSize.variant("large")}& {
    height: 3rem;
  }

  ${dataError.variant("")} & {
    border-color: ${colors.foreground.danger.primary};

    &:focus-within {
      box-shadow: ${shadows.error};
    }
  }
`
