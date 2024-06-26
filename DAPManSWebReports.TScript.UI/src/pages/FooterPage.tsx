import React        from 'react';
import { Icon    }  from '@rsuite/icons';
import { FaHeart }  from 'react-icons/fa';
import { BsChatDots as MessageIcon } from 'react-icons/bs';
import { FlexboxGrid } from 'rsuite';

const FooterPage = ({ handleReportClick }) => {
    
    return (
        <>
            <FlexboxGrid justify="center">
                Developed by [Shipelov]
                <Icon as={FaHeart} style={{ color: 'red', marginLeft: '5px' }} />   
                </FlexboxGrid>            
            <FlexboxGrid justify="end">
                <Icon as={MessageIcon} style={{marginRight: '5px', fontSize: '18px' }} />
                <a style={{marginRight: '15px', cursor: 'pointer' }} onClick={handleReportClick}>Сообщить об ошибке</a> 
            </FlexboxGrid>         
        </>
    );
};
export default FooterPage;