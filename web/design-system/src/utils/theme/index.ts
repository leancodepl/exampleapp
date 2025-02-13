import Color from "color"
import { ensureDefined } from "@leancodepl/utils"

type FilterConfig = {
  sepia: number
  grayscale: number
  contrast: number
  brightness: number
  mode: 0 | 1
}

function multiplyMatrices(m1: number[][], m2: number[][]) {
  const result: number[][] = []
  for (let i = 0, len = m1.length; i < len; i++) {
    result[i] = []
    for (let j = 0, len2 = m2[0].length; j < len2; j++) {
      let sum = 0
      for (let k = 0, len3 = m1[0].length; k < len3; k++) {
        sum += m1[i][k] * m2[k][j]
      }
      result[i][j] = sum
    }
  }
  return result
}

function createFilterMatrix(config: FilterConfig) {
  let m = Matrix.identity()
  if (config.sepia !== 0) {
    m = multiplyMatrices(m, Matrix.sepia(config.sepia / 100))
  }
  if (config.grayscale !== 0) {
    m = multiplyMatrices(m, Matrix.grayscale(config.grayscale / 100))
  }
  if (config.contrast !== 100) {
    m = multiplyMatrices(m, Matrix.contrast(config.contrast / 100))
  }
  if (config.brightness !== 100) {
    m = multiplyMatrices(m, Matrix.brightness(config.brightness / 100))
  }
  if (config.mode === 1) {
    m = multiplyMatrices(m, Matrix.invertNHue())
  }
  return m
}

function applyColorMatrix(color: string, matrix: number[][]) {
  const c = new Color(color)

  const result = multiplyMatrices(matrix, [...c.unitArray().map(v => [v]), [1], [1]])

  return new Color(result.map(x => x[0] * 255)).toString()
}

const Matrix = {
  identity() {
    return [
      [1, 0, 0, 0, 0],
      [0, 1, 0, 0, 0],
      [0, 0, 1, 0, 0],
      [0, 0, 0, 1, 0],
      [0, 0, 0, 0, 1],
    ]
  },

  invertNHue() {
    return [
      [0.333, -0.667, -0.667, 0, 1],
      [-0.667, 0.333, -0.667, 0, 1],
      [-0.667, -0.667, 0.333, 0, 1],
      [0, 0, 0, 1, 0],
      [0, 0, 0, 0, 1],
    ]
  },

  brightness(v: number) {
    return [
      [v, 0, 0, 0, 0],
      [0, v, 0, 0, 0],
      [0, 0, v, 0, 0],
      [0, 0, 0, 1, 0],
      [0, 0, 0, 0, 1],
    ]
  },

  contrast(v: number) {
    const t = (1 - v) / 2
    return [
      [v, 0, 0, 0, t],
      [0, v, 0, 0, t],
      [0, 0, v, 0, t],
      [0, 0, 0, 1, 0],
      [0, 0, 0, 0, 1],
    ]
  },

  sepia(v: number) {
    return [
      [0.393 + 0.607 * (1 - v), 0.769 - 0.769 * (1 - v), 0.189 - 0.189 * (1 - v), 0, 0],
      [0.349 - 0.349 * (1 - v), 0.686 + 0.314 * (1 - v), 0.168 - 0.168 * (1 - v), 0, 0],
      [0.272 - 0.272 * (1 - v), 0.534 - 0.534 * (1 - v), 0.131 + 0.869 * (1 - v), 0, 0],
      [0, 0, 0, 1, 0],
      [0, 0, 0, 0, 1],
    ]
  },

  grayscale(v: number) {
    return [
      [0.2126 + 0.7874 * (1 - v), 0.7152 - 0.7152 * (1 - v), 0.0722 - 0.0722 * (1 - v), 0, 0],
      [0.2126 - 0.2126 * (1 - v), 0.7152 + 0.2848 * (1 - v), 0.0722 - 0.0722 * (1 - v), 0, 0],
      [0.2126 - 0.2126 * (1 - v), 0.7152 - 0.7152 * (1 - v), 0.0722 + 0.9278 * (1 - v), 0, 0],
      [0, 0, 0, 1, 0],
      [0, 0, 0, 0, 1],
    ]
  },
}

const defaultDarkMatrixFilter = createFilterMatrix({
  brightness: 100,
  contrast: 80,
  grayscale: 0,
  mode: 1,
  sepia: 0,
})

export function defaultDarkFilterMap(light: string) {
  const parts = light.split(" ") // this is poors man box-shadow mapping. Probably need better solution though.

  return [...parts.slice(0, -1), applyColorMatrix(ensureDefined(parts.at(-1)), defaultDarkMatrixFilter)].join(" ")
}
