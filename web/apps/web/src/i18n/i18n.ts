/* eslint-disable @typescript-eslint/no-namespace */
import * as en from "./en.json"

export type EnTranslationsKeys = keyof typeof en

declare global {
    namespace FormatjsIntl {
        interface Message {
            ids: EnTranslationsKeys
        }
    }
}

export const enMessages = en

export const currentLocale = "en"
