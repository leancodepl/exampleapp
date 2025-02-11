import { styled } from "@pigment-css/react"
import { motion } from "motion/react"
import { colors, spacing } from "../../../utils"
import { dataFill } from "../attributes"

export const TabBarTabContainer = styled.button`
  position: relative;

  height: 40px;
  padding: ${spacing._2} ${spacing._4};

  color: ${colors.foreground.default.secondary};
  white-space: nowrap;
  text-align: center;
  text-decoration: none;

  background: none;
  border: none;
  cursor: pointer;

  ${dataFill.variant("")} & {
    flex: 1 1 auto;
  }

  &[data-state="active"] {
    color: ${colors.background.accent.primary};
  }
`

export const TabBarTabBar = styled(motion.div)`
  position: absolute;
  right: 0;
  bottom: -2px;
  left: 0;

  height: 2px;

  background: ${colors.background.accent.primary};
`
