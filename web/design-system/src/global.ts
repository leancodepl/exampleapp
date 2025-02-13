import { globalCss } from "@pigment-css/react"
import { colors } from "./utils"
import "modern-normalize/modern-normalize.css"

globalCss`
  body {
    font-size: 16px;
    width: 100vw;
    overflow-x: hidden;
    min-height: 100dvh;
    font-family: "Inter", sans-serif;
    background: ${colors.background.default.primary};
  }

  ol, ul {
    list-style: none;
    margin: 0;
    padding: 0;
  }

  a {
    color: ${colors.background.accent.primary};
  }
`
