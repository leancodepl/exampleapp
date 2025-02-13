import {
  ComponentPropsWithoutRef,
  createContext,
  forwardRef,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react"
import { useIsMobile } from "../../../utils/hooks/useResponsive"
import { SidebarWrapper } from "./styles"

type SidebarContextData = {
  state: "collapsed" | "expanded"
  open: boolean
  setOpen: (open: boolean) => void
  openMobile: boolean
  setOpenMobile: (open: boolean) => void
  isMobile: boolean
  toggleSidebar: () => void
}

const SidebarContext = createContext<SidebarContextData | null>(null)

export function useSidebar() {
  const context = useContext(SidebarContext)

  if (!context) {
    throw new Error("useSidebar must be used within a SidebarProvider.")
  }

  return context
}

type SidebarProviderProps = {
  defaultOpen?: boolean
  open?: boolean
  onOpenChange?: (open: boolean) => void
} & ComponentPropsWithoutRef<"div">

export const SidebarProvider = forwardRef<HTMLDivElement, SidebarProviderProps>(
  ({ defaultOpen, open: openProp, onOpenChange: setOpenProp, children, ...props }, ref) => {
    const isMobile = useIsMobile()
    const [openMobile, setOpenMobile] = useState(false)

    // This is the internal state of the sidebar.
    // We use openProp and setOpenProp for control from outside the component.
    const [_open, _setOpen] = useState(() => defaultOpen ?? localStorage.getItem(SIDEBAR_LOCAL_STORAGE_KEY) !== "false")
    const open = openProp ?? _open

    const state = open ? "expanded" : "collapsed"

    const setOpen = useCallback(
      (value: ((value: boolean) => boolean) | boolean) => {
        const newValue = typeof value === "function" ? value(open) : value
        localStorage.setItem(SIDEBAR_LOCAL_STORAGE_KEY, newValue ? "true" : "false")

        if (setOpenProp) {
          return setOpenProp(newValue)
        }

        _setOpen(newValue)
      },
      [setOpenProp, open],
    )

    const toggleSidebar = useCallback(
      () => (isMobile ? setOpenMobile(open => !open) : setOpen(open => !open)),
      [isMobile, setOpen, setOpenMobile],
    )

    useEffect(() => {
      const handleKeyDown = (event: KeyboardEvent) => {
        if (event.key === SIDEBAR_KEYBOARD_SHORTCUT && (event.metaKey || event.ctrlKey)) {
          event.preventDefault()
          toggleSidebar()
        }
      }

      window.addEventListener("keydown", handleKeyDown)

      return () => window.removeEventListener("keydown", handleKeyDown)
    }, [toggleSidebar])

    const contextValue = useMemo<SidebarContextData>(
      () => ({
        state,
        open,
        setOpen,
        isMobile,
        openMobile,
        setOpenMobile,
        toggleSidebar,
      }),
      [state, open, setOpen, isMobile, openMobile, setOpenMobile, toggleSidebar],
    )

    return (
      <SidebarContext.Provider value={contextValue}>
        <SidebarWrapper ref={ref} {...props}>
          {children}
        </SidebarWrapper>
      </SidebarContext.Provider>
    )
  },
)

const SIDEBAR_LOCAL_STORAGE_KEY = "sidebarOpen"
const SIDEBAR_KEYBOARD_SHORTCUT = "b"
