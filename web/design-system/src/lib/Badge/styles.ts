import { styled } from "@pigment-css/react"
import { colors, spacing } from "../../utils"
import { dataType } from "./attributes"

export const BadgeRoot = styled("div", {
  name: "Badge",
  slot: "root",
})`
  display: inline-flex;
  flex: 0 0 auto;
  gap: ${spacing._1};
  align-items: center;
  height: 1.5rem;
  padding: ${spacing._1} ${spacing._2};

  border-radius: 1000px;

  &${dataType.variant("info")} {
    color: ${colors.foreground.info.primary};

    background: ${colors.background.info.tertiary};
  }

  &${dataType.variant("success")} {
    color: ${colors.foreground.success.primary};

    background: ${colors.background.success.tertiary};
  }

  &${dataType.variant("warning")} {
    color: ${colors.foreground.warning.primary};

    background: ${colors.background.warning.tertiary};
  }

  &${dataType.variant("danger")} {
    color: ${colors.foreground.danger.primary};

    background: ${colors.background.danger.tertiary};
  }

  &${dataType.variant("neutral")} {
    color: ${colors.foreground.default.primary};

    background: ${colors.background.default.secondary};
  }
`
