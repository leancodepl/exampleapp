export function attribute<TValue extends string>(name: string) {
  const dataAttributeName = name

  function attr(value?: TValue): Partial<Record<`data-${string}`, TValue>> {
    if (value === undefined) return {}

    return { [`data-${dataAttributeName}`]: value }
  }

  attr.key = dataAttributeName
  attr.variant = (value: TValue) => () => {
    if (value === "") return `[data-${dataAttributeName}]`

    return `[data-${dataAttributeName}="${value}"]`
  }

  return attr
}
