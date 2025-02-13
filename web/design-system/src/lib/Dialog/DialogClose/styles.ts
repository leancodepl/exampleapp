import { styled } from "@pigment-css/react"
import { breakpoints, colors, spacing } from "../../../utils"
import { Button } from "../../Button"

export const DialogCloseButton = styled(Button, { name: "Dialog", slot: "closeButton" })`
  position: absolute;
  top: ${spacing._3};
  right: ${spacing._3};

  color: ${colors.foreground.default.secondary};

  ${breakpoints.down.xs} {
    top: ${spacing._6};
  }
`
