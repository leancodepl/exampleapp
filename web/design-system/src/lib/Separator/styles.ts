import { styled } from "@pigment-css/react"
import { colors, spacing } from "../../utils"

export const SeparatorRoot = styled.div`
  &:not(:empty) {
    display: grid;
    grid-template-columns: 1fr auto 1fr;
    align-items: center;

    color: ${colors.foreground.default.secondary};

    &:before,
    &:after {
      height: 1px;

      background: ${colors.foreground.default.tertiary};

      content: "";
    }

    &:before {
      margin-right: ${spacing._4};
    }

    &:after {
      margin-left: ${spacing._4};
    }
  }

  &:empty {
    height: 1px;

    background: ${colors.foreground.default.tertiary};
  }
`
