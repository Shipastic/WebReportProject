import React, { useState } from 'react';
import Joyride, { CallBackProps, STATUS, Step } from 'react-joyride';

const steps: Step[] = [
  {
    target: '.queryparamscomponentStyle',
    content: 'Необходимо выбрать параметры формирования отчета. Нажми на кнопку!',
  },
  {
    target: '.queryparamsStyle',
    content: 'Фильтры помогут выбрать нужный временной диапазон или выбрать предустановленное.',
  },
  {
    target: '.content',
    content: 'This is the main content area where you can work on your projects.',
  },
];

const StepsTutorialComponent: React.FC = () => {
  const [run, setRun] = useState(true);

  const handleJoyrideCallback = (data: CallBackProps) => {
    const { status } = data;

    if (status === STATUS.FINISHED || status === STATUS.SKIPPED) {
      setRun(false);
    }
  };

  return (
    <Joyride
      steps={steps}
      run={run}
      continuous
      showSkipButton
      callback={handleJoyrideCallback}
    />
  );
};

export default StepsTutorialComponent;