import { FormattedMessage } from "react-intl"
import { Link, useMatch } from "react-router-dom"
import { DatabaseOutlined, LogoutOutlined, SettingOutlined, TeamOutlined } from "@ant-design/icons"
import { ItemType, MenuItemType } from "antd/es/menu/interface"
import { useLogoutFlow } from "@leancodepl/kratos"
import { useKeyByRoute } from "../../../_hooks/useKeyByRoute"
import { kratosClient } from "../../../auth"
import { sessionManager } from "../../../auth/sessionManager"
import { path } from "../../../routes"
import { MenuContainer, MenuWithoutBorder } from "./styles"

export function NavMenu() {
    const { logout } = useLogoutFlow({ kratosClient, onLoggedOut: () => sessionManager.checkIfLoggedIn() })

    const [activeKey] = useKeyByRoute({
        [MenuKeys.Projects]: useMatch(path("projects", "index")),
        [MenuKeys.Settings]: useMatch(path("settings")),
        [MenuKeys.Employees]: useMatch(path("employees", "index")),
    })

    return (
        <MenuContainer>
            <MenuWithoutBorder activeKey={activeKey} items={items} selectedKeys={[activeKey]} />
            <MenuWithoutBorder
                items={[
                    {
                        key: "sign-out",
                        danger: true,
                        icon: <LogoutOutlined />,
                        label: <FormattedMessage defaultMessage="Sign out" />,
                        onClick: logout,
                    },
                ]}
            />
        </MenuContainer>
    )
}

enum MenuKeys {
    Projects = "projects",
    Settings = "settings",
    Employees = "employees",
}

const items: ItemType<MenuItemType>[] = [
    {
        key: MenuKeys.Projects,
        icon: <DatabaseOutlined />,
        label: (
            <Link to={path("projects", "index")}>
                <FormattedMessage defaultMessage="Projects" />
            </Link>
        ),
    },
    {
        key: MenuKeys.Employees,
        icon: <TeamOutlined />,
        label: (
            <Link to={path("employees", "index")}>
                <FormattedMessage defaultMessage="Employees" />
            </Link>
        ),
    },
    {
        key: MenuKeys.Settings,
        icon: <SettingOutlined />,
        label: (
            <Link to={path("settings")}>
                <FormattedMessage defaultMessage="Settings" />
            </Link>
        ),
    },
]
