import { styled } from "@pigment-css/react"
import * as SelectPrimitive from "@radix-ui/react-select"
import { colors, radii, spacing } from "../../../utils"

export const SelectPrimitiveItem = styled(SelectPrimitive.Item, { name: "Select", slot: "item" })`
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 48px;
  padding: ${spacing._3} ${spacing._4};

  color: ${colors.foreground.default.secondary};

  background: ${colors.background.default.primary};
  cursor: pointer;

  &:hover,
  &:focus {
    background: ${colors.background.default.primary_hover};
    outline: none;
  }

  &:last-child {
    border-bottom-right-radius: ${radii.md};
    border-bottom-left-radius: ${radii.md};
  }

  svg {
    color: ${colors.foreground.default.tertiary};
  }

  [data-state="checked"]& {
    color: ${colors.foreground.default.primary};

    svg {
      color: ${colors.foreground.default.secondary};
    }
  }

  & > span:first-child {
    display: flex;
    gap: ${spacing._2};
    align-items: center;
    overflow: hidden;
  }
`
