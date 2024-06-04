import './Modal.css'; 
import React from 'react';
import { Button } from 'rsuite';

interface ModalWindowProps {
    isOpen: boolean;
    close: () => void;
    onContinue: () => void;
}

const ModalWindow: React.FC<ModalWindowProps> = ({ isOpen, close, onContinue }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={close}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <h2>Предупреждение!!!</h2>
        <p>Вы запускаете выполнение запроса без фильтров!!!</p>
              <Button onClick={close}>Добавить параметры</Button>
              <Button onClick={onContinue}>Продолжить без параметров</Button>
      </div>
    </div>
  );
};

export default ModalWindow;