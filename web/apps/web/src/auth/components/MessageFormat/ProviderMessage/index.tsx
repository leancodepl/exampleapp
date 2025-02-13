import { ReactNode } from "react"
import { ThemeComponent } from "../../../../components/ThemeComponent"
import IconAppleDark from "./icon_apple_dark.svg?react"
import IconAppleLight from "./icon_apple_light.svg?react"
import IconFacebookDark from "./icon_facebook_dark.svg?react"
import IconFacebookLight from "./icon_facebook_light.svg?react"
import IconGoogleDark from "./icon_google_dark.svg?react"
import IconGoogleLight from "./icon_google_light.svg?react"

type ProviderMessageProps = {
  provider: string
  fallback?: ReactNode
}

export function ProviderMessage({ provider, fallback }: ProviderMessageProps) {
  switch (provider) {
    case "Facebook":
    case "facebook":
      return (
        <>
          <ThemeComponent dark={<IconFacebookDark />} light={<IconFacebookLight />} />
          Facebook
        </>
      )
    case "Google":
    case "google":
      return (
        <>
          <ThemeComponent dark={<IconGoogleDark />} light={<IconGoogleLight />} />
          Google
        </>
      )
    case "Apple":
    case "apple":
      return (
        <>
          <ThemeComponent dark={<IconAppleDark />} light={<IconAppleLight />} />
          Apple
        </>
      )
  }

  return <>{fallback}</>
}
