import { ThemeArgs } from "@pigment-css/react/build/theme"

type Value = { [key: string | symbol]: Value | undefined } | number | string | undefined

function guard(value: unknown): value is Record<string | symbol, unknown> {
  return typeof value === "object"
}

function mkProxy(accessor: (themeArgs: ThemeArgs) => Value) {
  const cache: Record<string | symbol, ReturnType<typeof mkProxy>> = {}

  return new Proxy(accessor, {
    get(target, property) {
      if (property === "prototype") {
        return Function.prototype
      }

      cache[property] ??= mkProxy((themeArgs: ThemeArgs) => {
        const value = target(themeArgs)

        return guard(value) ? value?.[property] : undefined
      })

      return cache[property]
    },
  })
}

type TransformDeep<T> =
  T extends Array<infer TArrayElement>
    ? Array<TransformDeep<TArrayElement>>
    : T extends object
      ? {
          [TKey in keyof T]: TransformDeep<T[TKey]>
        }
      : T extends null
        ? undefined
        : (themeArgs: ThemeArgs) => T

export const colors = mkProxy(({ theme }) => theme.vars.colors) as unknown as TransformDeep<
  ThemeArgs["theme"]["vars"]["colors"]
>
export const spacing = mkProxy(({ theme }) => theme.spacing) as unknown as TransformDeep<ThemeArgs["theme"]["spacing"]>
export const radii = mkProxy(({ theme }) => theme.radii) as unknown as TransformDeep<ThemeArgs["theme"]["radii"]>
export const shadows = mkProxy(({ theme }) => theme.shadows) as unknown as TransformDeep<ThemeArgs["theme"]["shadows"]>
export const breakpoints = mkProxy(({ theme }) => theme.breakpoints) as unknown as TransformDeep<
  ThemeArgs["theme"]["breakpoints"]
>
