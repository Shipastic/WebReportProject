import React, { useState, useEffect } from "react";
import '../App.css';
import "rsuite/dist/rsuite.css";
import { Navbar, Nav, Dropdown } from 'rsuite';
import CogIcon from '@rsuite/icons/legacy/Cog';
import 'rsuite/dist/rsuite.min.css';
import { FolderDetail } from "../Models/FolderDetail";
import ChildFolderComponent from "../Components/ChildFolderComponent";
interface HomePageProps
{
    onSelect  ?: (eventKey: any) => void;
    activeKey ?: string;
    [key: string]: any; 
}
const HomePage: React.FC<HomePageProps> = ({ onSelect, activeKey, ...props }) =>
{
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [items, setItems] = useState<FolderDetail | null>(null);
    const [isFolderComponentVisible, setFolderComponentVisible] = useState<boolean>(false);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

    useEffect(() => {
        fetch('https://localhost:7263/api/menu/parents')
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Network response was not ok.');
            })
            .then(data => setItems(data))
            .catch(error => console.error("There was an error fetching the reports: ", error));
    }, []);

    const handleDropdownSelect = () =>
    {
        setFolderComponentVisible(!isFolderComponentVisible);
    };

    const handleItemClick = (id: number) =>
    {
        setActiveFolderId(id);
    };
    const handleDataViewClick = (dataviewid: number) =>
    {
        setSelectedDataViewId(dataviewid);
    };
    return (      
        <>
            <Navbar {...props}>
                <Navbar.Brand href="home">Главная</Navbar.Brand>
                <Nav onSelect={onSelect} activeKey={activeKey}>
                    <Nav.Item eventKey="/login">Войти</Nav.Item>
                    <Nav.Item eventKey="/signup">Зарегистрироваться</Nav.Item>         
                        <Dropdown.Menu onSelect={handleDropdownSelect} title={"Выберите цех"}>
                            {items?.childFolders.map(childFolder => (
                                    <Dropdown.Item
                                        key={childFolder.id}
                                        eventKey={childFolder.id}
                                        onSelect={() => handleItemClick(childFolder.id)}
                                    >
                                        {childFolder.name}
                                    </Dropdown.Item>
                                ))}
                            <hr />
                            {items?.dataviews.map(dataview => (
                                    <Dropdown.Item
                                        key={dataview.id}
                                        onClick={() => handleDataViewClick(dataview.id)}
                                    >
                                        {dataview.name}
                                    </Dropdown.Item>
                                ))}
                        </Dropdown.Menu>
                </Nav>
                <Nav pullRight>
                    <Nav.Item icon={<CogIcon />}>Settings</Nav.Item>
                </Nav>
        </Navbar >           
            {activeFolderId !== null && (
                <ChildFolderComponent
                    parentid={activeFolderId} />
            )}            
            {/*
            {isFolderComponentVisible !== null && (
                <ChildFolderComponent
                    parentid={activeFolderId} />
            )}
            */}
        </>
    );
};

export default HomePage;

