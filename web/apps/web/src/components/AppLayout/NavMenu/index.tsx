import { DatabaseOutlined, LogoutOutlined, SettingOutlined } from "@ant-design/icons";
import { useLogoutFlow } from "@leancodepl/kratos";
import type { ItemType, MenuItemType } from "antd/es/menu/hooks/useItems";
import { FormattedMessage } from "react-intl";
import { Link, useMatch } from "react-router-dom";
import { MenuContainer, MenuWithoutBorder } from "./styles";
import { useKeyByRoute } from "../../../_hooks/useKeyByRoute";
import { kratosClient } from "../../../auth";
import { sessionManager } from "../../../auth/sessionManager";
import { path } from "../../../routes";

export function NavMenu() {
    const { logout } = useLogoutFlow({ kratosClient, onLoggedOut: () => sessionManager.checkIfLoggedIn() });

    const [activeKey] = useKeyByRoute({
        [MenuKeys.Projects]: useMatch(path("projects", "index")),
        [MenuKeys.Settings]: useMatch(path("settings")),
    });

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
    );
}

enum MenuKeys {
    Projects = "projects",
    Settings = "settings",
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
        key: MenuKeys.Settings,
        icon: <SettingOutlined />,
        label: (
            <Link to={path("settings")}>
                <FormattedMessage defaultMessage="Settings" />
            </Link>
        ),
    },
];
