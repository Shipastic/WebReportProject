import React from 'react';
import { Icon } from '@rsuite/icons';
import { FaHeart } from 'react-icons/fa';
import MessageIcon from '@rsuite/icons/Message';

const FooterPage = () => {
    return (
        <footer className="custom-footer" style={{paddingBottom:'9px'}} >
            <div style={{ textAlign: 'center', color: '#fff'}} >
                <div>
                    Developed by [Shipelov]
                    <Icon as={FaHeart} style={{ color: 'red', marginLeft: '5px' }} />   
                </div>             
                <div style={{ textAlign: 'right', marginTop:'-10px'}}>
                    <Icon as={MessageIcon} style={{marginRight: '5px', fontSize: '18px' }} />
                    <a href="shipelov_de@omk.ru" style={{marginRight: '15px'}}>Сообщить об ошибке</a> 
                </div>       
            </div>  
        </footer>
    );
};
export default FooterPage;