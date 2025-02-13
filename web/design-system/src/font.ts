import { globalCss } from "@pigment-css/react"
import InterItalic from "./fonts/Inter-Italic-VariableFont_opsz,wght.ttf"
import Inter from "./fonts/Inter-VariableFont_opsz,wght.ttf"

globalCss`
  @font-face {
    font-family: "Inter";
    font-weight: 100 900;
    font-style: normal;
    src: url(${Inter}) format("truetype-variations");
    font-display: swap;
  }

  @font-face {
    font-family: "Inter";
    font-weight: 100 900;
    font-style: italic;
    src: url(${InterItalic}) format("truetype-variations");
    font-display: swap;
  }
`
