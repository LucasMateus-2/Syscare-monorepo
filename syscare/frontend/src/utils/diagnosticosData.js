// Gerado a partir dos dados originais das fichas da clínica.
const DADOS = {
  "DIAGNOSTICOS": {
    "integridade_pele": {
      "nome": "Integridade da pele prejudicada",
      "definicao": "Dano à epiderme e/ou à derme.",
      "caracteristicas_definidoras": [
        "Abscesso",
        "Área localizada quente ao toque",
        "Bolha",
        "Cor da pele alterada",
        "Descamação",
        "Dor aguda",
        "Escoriação",
        "Hematoma",
        "Hiperceratose",
        "Material estranho perfurando a pele",
        "Pele com abrasão",
        "Pele macerada",
        "Pele ressecada",
        "Prurido",
        "Sangramento",
        "Superfície cutânea rompida",
        "Turgor alterado",
        "Ulceração"
      ],
      "fatores_relacionados": [
        "Atrito em superfície",
        "Conhecimento inadequado do cuidador sobre a manutenção da integridade tissular",
        "Conhecimento inadequado do cuidador sobre o uso apropriado de materiais adesivos",
        "Excreções",
        "Forças de cisalhamento",
        "Nível inapropriado de umidade da pele",
        "Pressão sobre proeminência óssea",
        "Secreções",
        "Alergia a adesivos não abordada",
        "Atividade física diminuída",
        "Autogestão ineficaz do sobrepeso",
        "Baixo peso para a idade e o sexo",
        "Conhecimento inadequado sobre a manutenção da integridade tissular",
        "Conhecimento inadequado sobre a proteção da integridade tissular",
        "Conhecimento inadequado sobre o uso apropriado de materiais adesivos",
        "Edema",
        "Mobilidade física prejudicada",
        "Uso de tabaco"
      ],
      "noc_titulo": "Integridade da pele e mucosas",
      "noc_escala": [
        "Nenhum",
        "Limitado",
        "Moderado",
        "Substancial",
        "Extenso"
      ],
      "noc_indicadores": [
        "Granulação",
        "Formação de cicatriz",
        "Drenagem serosa",
        "Drenagem sanguinolenta",
        "Drenagem purulenta",
        "Eritema na pele adjacente",
        "Edema perilesão",
        "Pele macerada",
        "Necrose",
        "Odor fétido"
      ],
      "nic_titulo": "Cuidados com lesões",
      "nic_atividades": [
        "Monitorar as características da lesão, incluindo drenagem, cor, tamanho e odor.",
        "Medir o leito da lesão conforme apropriado.",
        "Trocar a cobertura conforme a quantidade de exsudato e drenagem.",
        "Examinar a lesão a cada troca de cobertura.",
        "Orientar o paciente e familiares sobre procedimentos de cuidados com a lesão.",
        "Orientar o paciente e seus familiares quanto a sinais e sintomas de infecção."
      ]
    },
    "dor": {
      "nome": "Dor (aguda/crônica)",
      "definicao": "Experiência sensorial e emocional desagradável, associada ou semelhante à associada a um dano tecidual real ou potencial.",
      "subtipos": [
        "Dor aguda (duração inferior a 3 meses)",
        "Dor crônica (duração superior a 3 meses)"
      ],
      "caracteristicas_definidoras": [
        "Ciclo sono-vigília alterado",
        "Expressão facial de dor",
        "Habilidade alterada de continuar as atividades",
        "Hipervigilância à dor",
        "Intensidade da dor avaliada por um instrumento de avaliação padronizado e validado",
        "Relato verbal de dor",
        "Representante relata comportamento de dor",
        "Representante relata mudanças nas atividades",
        "Comportamentos protetores"
      ],
      "fatores_relacionados": [
        "Autogestão ineficaz do sobrepeso",
        "Dificuldade em estabelecer interação social",
        "Sofrimento psicológico"
      ],
      "noc_titulo": "Nível de dor",
      "noc_escala": null,
      "noc_indicadores": [
        "Dor relatada",
        "Duração dos episódios de dor",
        "Esfrega a área afetada",
        "Expressões faciais de dor",
        "Irritabilidade",
        "Inquietação",
        "Retraimento",
        "Agitação"
      ],
      "nic_titulo": "Controle da dor",
      "nic_atividades": [
        "Realizar avaliação abrangente de dor, incluindo localização, início, duração, frequência e intensidade da dor, bem como fatores de melhora e desencadeantes.",
        "Observar sinais de desconforto, principalmente naqueles que não conseguem se comunicar efetivamente.",
        "Incorporar intervenções não farmacológicas conforme a etiologia da dor e a preferência do paciente, conforme apropriado (laserterapia).",
        "Modificar as medidas de controle da dor com base na resposta do paciente ao tratamento (laserterapia).",
        "Monitorar a dor utilizando um instrumento de classificação válido e confiável apropriado para a idade e a capacidade de comunicação."
      ]
    },
    "glicemia": {
      "nome": "Risco de autogestão ineficaz do padrão de glicemia",
      "definicao": "Suscetibilidade a manejo insatisfatório de sintomas, regime terapêutico e mudanças no estilo de vida associados à convivência com flutuações recorrentes no nível de glicose sanguínea fora da faixa desejável.",
      "fatores_relacionados": [
        "Autogestão ineficaz de medicamentos",
        "Autogestão ineficaz do sobrepeso",
        "Automonitoração inadequada da glicemia",
        "Comportamentos sedentários",
        "Comprometimento inadequado com um plano de ação",
        "Conhecimento inadequado dos fatores modificáveis",
        "Conhecimento inadequado sobre o manejo de doenças",
        "Conhecimento inadequado sobre o regime terapêutico",
        "Dificuldade em realizar aspectos do regime terapêutico",
        "Estresse excessivo",
        "Falta de consciência da gravidade da condição",
        "Falta de consciência da suscetibilidade a sequelas",
        "Gestão do peso ineficaz",
        "Letramento em saúde inadequado",
        "Uso de tabaco"
      ],
      "noc_titulo": "Gravidade da hiperglicemia / hipoglicemia",
      "noc_escala": [
        "Grave",
        "Substancial",
        "Moderado",
        "Leve",
        "Nenhum"
      ],
      "noc_indicadores": [
        "Débito urinário aumentado (Poliúria)",
        "Sede aumentada (Polidipsia)",
        "Fome excessiva (Polifagia)",
        "Fadiga",
        "Cefaleia",
        "Visão embaçada",
        "Glicose sanguínea elevada"
      ],
      "nic_titulo": "Controle da hiperglicemia",
      "nic_atividades": [
        "Monitorar níveis de glicose sanguínea, conforme indicado.",
        "Monitorar sinais e sintomas de hiperglicemia: poliúria, polidipsia, polifagia, fraqueza, letargia, mal-estar, embaçamento visual ou dor de cabeça.",
        "Orientar o paciente e pessoas significativas sobre prevenção, reconhecimento e controle da hiperglicemia.",
        "Incentivar a automonitoração dos níveis de glicose no sangue.",
        "Revisar os registros de glicose sanguínea com o paciente e/ou família (se solicitado controle)."
      ]
    },
    "risco_infeccao": {
      "nome": "Risco de Infecção",
      "definicao": "Suscetibilidade à invasão e multiplicação de organismos patogênicos.",
      "fatores_relacionados": [
        "Autogestão ineficaz do sobrepeso",
        "Conhecimento inadequado sobre como evitar exposição a patógenos",
        "Desnutrição",
        "Higiene ambiental inadequada",
        "Integridade da pele prejudicada",
        "Letramento em saúde inadequado",
        "Práticas inadequadas de higiene pessoal",
        "Resposta imune prejudicada",
        "Uso de tabaco"
      ],
      "noc_titulo": "Gravidade da infecção",
      "noc_escala": null,
      "noc_indicadores": [
        "Erupção cutânea",
        "Drenagem purulenta",
        "Dor",
        "Edema Periférico",
        "Dor localizada nas extremidades",
        "Supuração de odor desagradável",
        "Febre"
      ],
      "nic_titulo": "Controle de infecção",
      "nic_atividades": [
        "Limpar o ambiente apropriadamente após o uso de cada paciente.",
        "Lavar as mãos antes e depois da atividade de atendimento de cada paciente.",
        "Ensinar ao paciente e à família a respeito dos sinais e sintomas da infecção e quando notificá-los ao profissional da saúde.",
        "Ensinar ao paciente e membros da família como evitar infecções."
      ]
    },
    "ansiedade": {
      "nome": "Ansiedade",
      "definicao": "Preocupação desproporcional e persistente com situações e eventos percebidos como ameaçadores.",
      "caracteristicas_definidoras": [
        "Humor irritável",
        "Insegurança",
        "Insônia",
        "Nervosismo",
        "Preocupação com mudanças em eventos da vida",
        "Produtividade diminuída",
        "Pressão arterial aumentada",
        "Rubor facial",
        "Tensão",
        "Preocupação"
      ],
      "fatores_relacionados": [
        "Conflito sobre as metas da vida",
        "Dor",
        "Estresse excessivo",
        "Necessidades não atendidas",
        "Situação desconhecida"
      ],
      "noc_titulo": "Nível de ansiedade",
      "noc_escala": null,
      "noc_indicadores": [
        "Inquietação",
        "Nervosismo",
        "Preocupação excessiva",
        "Sentindo-se sem valor",
        "Dor localizada nas extremidades",
        "Dor",
        "Ansiedade verbalizada",
        "Preocupação exagerada sobre eventos de vida",
        "Pressão arterial aumentada"
      ],
      "nic_titulo": "Intervenções gerais",
      "nic_atividades": [
        "Utilizar abordagem calma e tranquilizadora.",
        "Buscar compreender a perspectiva do paciente quanto à situação estressante.",
        "Escutar atentamente.",
        "Criar uma atmosfera para facilitar a confiança.",
        "Encorajar a verbalização dos sentimentos, das percepções e dos medos.",
        "Identificar mudanças no nível de ansiedade.",
        "Avaliar sinais verbais e não verbais de ansiedade."
      ]
    },
    "autogestao_saude": {
      "nome": "Autogestão ineficaz da saúde",
      "definicao": "Manejo insatisfatório de sintomas, regime terapêutico e mudanças no estilo de vida associados a viver com uma doença crônica.",
      "caracteristicas_definidoras": [
        "Apresenta sequelas da doença",
        "Desatenção aos sinais da doença",
        "Desatenção aos sintomas da doença",
        "Escolhas da vida diária ineficazes para atingir as metas de saúde",
        "Exacerbação dos sinais da doença",
        "Exacerbação dos sintomas da doença",
        "Falha em agir de forma a reduzir fatores de risco",
        "Falha em comparecer a compromissos agendados com profissional de saúde",
        "Falha em incluir o regime terapêutico à vida diária",
        "Insatisfação com a qualidade de vida"
      ],
      "fatores_relacionados": [
        "Apoio social inadequado",
        "Autoeficácia inadequada",
        "Comprometimento inadequado com um plano de ação",
        "Conflito entre comportamentos de saúde e normas sociais",
        "Conhecimento inadequado sobre o regime terapêutico",
        "Demandas concorrentes",
        "Estresse excessivo",
        "Expectativa não realista em relação ao benefício do tratamento",
        "Falta de consciência da gravidade da condição",
        "Falta de consciência da suscetibilidade a sequelas",
        "Letramento em saúde inadequado",
        "Qualidade de vida diminuída",
        "Sentimentos negativos em relação ao regime terapêutico",
        "Sintomas depressivos"
      ],
      "noc_titulo": "Autocontrole: doença crônica",
      "noc_escala": [
        "Desvio grave da variação normal",
        "Desvio substancial da variação normal",
        "Desvio moderado da variação normal",
        "Desvio leve da variação normal",
        "Sem desvio da variação normal"
      ],
      "noc_indicadores": [
        "Aceita o diagnóstico",
        "Busca informações sobre a doença",
        "Monitora sinais e sintomas da doença",
        "Segue o tratamento recomendado",
        "Utiliza dispositivos terapêuticos corretamente (ex. terapia compressiva)",
        "Segue o regime medicamentoso",
        "Monitora os sinais vitais",
        "Mantém as consultas com o profissional de saúde",
        "Segue a dieta recomendada"
      ],
      "nic_titulo": "Ensino: processo da doença",
      "nic_atividades": [
        "Revisar o conhecimento do paciente sobre a doença.",
        "Descrever os sinais e sintomas comuns da doença, conforme apropriado.",
        "Fornecer informações ao paciente sobre a doença, conforme apropriado.",
        "Discutir as mudanças de estilo de vida que podem ser necessárias para evitar futuras complicações e/ou controlar o processo da doença.",
        "Identificar alterações na condição física do paciente.",
        "Discutir as opções de terapia/tratamento.",
        "Reforçar as informações fornecidas por outros membros da equipe de saúde, conforme apropriado."
      ]
    },
    "perfusao_perifericca": {
      "nome": "Perfusão tissular periférica ineficaz",
      "definicao": "Diminuição da circulação sanguínea para as extremidades.",
      "caracteristicas_definidoras": [
        "Ausência de pulsos periféricos",
        "Ausência de sudorese nas extremidades",
        "Cianose em extremidade",
        "Cicatrização de ferida periférica atrasada",
        "Claudicação intermitente",
        "Cor não volta ao membro inferior abaixado após permanecer 1 minuto",
        "Elevado",
        "Dor em extremidade",
        "Edema",
        "Empalidecimento da cor da extremidade à elevação de membro",
        "Extremidades frias",
        "Função motora alterada",
        "Índice tornozelo-braquial < 0,90",
        "Parestesia",
        "Pulsos periféricos diminuídos",
        "Sudorese diminuída nas extremidades",
        "Tempo de enchimento capilar > 3 segundos",
        "Unhas distróficas"
      ],
      "fatores_relacionados": [
        "Sudorese diminuída nas extremidades",
        "Tempo de enchimento capilar > 3 segundos",
        "Unhas distróficas"
      ],
      "noc_titulo": "Perfusão tissular periférica",
      "noc_escala": null,
      "noc_indicadores": [
        "Enchimento capilar nos dedos dos pés",
        "Temperatura da pele nas extremidades",
        "Força do pulso pedial (E) (D)",
        "Edema Periférico",
        "Dor localizada nas extremidades",
        "Necrose",
        "Rubor",
        "Ruptura da pele",
        "Capacidade de sentir estímulos na pele",
        "Formigamento"
      ],
      "nic_titulo": "Cuidados com a perfusão periférica",
      "nic_atividades": [
        "Cuidados com os pés.",
        "Controle da dor.",
        "Regulação da temperatura.",
        "Monitorização de SSVV.",
        "Controle da sensibilidade periférica."
      ]
    }
  },
  "DOENCA_BASE_OPCOES": [
    "Diabetes mellitus",
    "Hipertensão arterial sistêmica",
    "Doenças vascular periférica (venosa ou arterial)",
    "Cardiopatia",
    "Doença renal",
    "Doença respiratória",
    "Neuropatia",
    "Distúrbios de coagulação",
    "Obesidade",
    "Doenças imunossupressoras",
    "Outras"
  ],
  "HABITOS_OPCOES": [
    "Tabagismo",
    "Etilismo"
  ],
  "SONO_OPCOES": [
    "Dorme bem",
    "Dorme pouco",
    "Insônia"
  ],
  "TIME_TECIDO_OPCOES": [
    "Granulação",
    "Necrose de coagulação (seca)",
    "Necrose de liquefação (úmida)",
    "Esfacelo",
    "Hipergranulação",
    "Granulação não saudável (friável/ escurecida)"
  ],
  "TIME_INFECCAO_SUPERFICIAL": [
    "U – Tecido inviável (presença de > 50% de tecido necrótico ou tecido de granulação friável/descoloração)",
    "P – Dor nova ou crescente",
    "P – Atraso da cicatrização",
    "E – Exsudato (moderada a grande quantidade)",
    "R – Odor (presença de mau odor)"
  ],
  "TIME_INFECCAO_PROFUNDA": [
    "L – Aumento no tamanho da ferida ou aparição de lesões satélites",
    "O – Exposição óssea",
    "W – Aumento da temperatura periferida",
    "E – Presença de leve a moderado edema",
    "R – Eritema > 2 cm além da margem da ferida"
  ],
  "EXSUDATO_TIPOS": [
    [
      "Seroso",
      "Claro",
      "Aquosa"
    ],
    [
      "Serosanguinolento",
      "Vermelho claro/ rosado",
      "Aquosa"
    ],
    [
      "Sanguinolento",
      "Avermelhado",
      "Espesso"
    ],
    [
      "Purulento",
      "Amarelado/ esverdeado",
      "Espesso"
    ],
    [
      "Seropurulento",
      "Amarelado",
      "Leitoso (fino)"
    ],
    [
      "Hemopurulento",
      "Avermelhado",
      "Espesso"
    ]
  ],
  "EXSUDATO_QUANTIDADE": [
    "Ausente",
    "Pequena",
    "Moderada",
    "Grande"
  ],
  "BORDAS_OPCOES": [
    "Maceração",
    "Hiperqueratose",
    "Descolamento",
    "Epíbole",
    "Íntegra (aderida, nivelada, sinais de epitelização)"
  ],
  "PERILESIONAL_OPCOES": [
    "Íntegra",
    "Eczema (descamação)",
    "Hiperpigmentação (dermatite ocre)",
    "Hiperemiada",
    "Ressecada"
  ],
  "BIOFILME_SINAIS": [
    "Substância viscosa, espessa e brilhante na superfície da ferida",
    "Pigmentação amarelada e/ou esverdeada",
    "Material gelatinoso que se reforma rapidamente após uma ação mecânica (em 24 - 48h)",
    "Falha no tratamento apesar do uso de antimicrobianos adequados",
    "Atraso na cicatrização",
    "Ciclos de infecções recorrentes",
    "Aumento na quantidade de exsudato",
    "Tecido de granulação friável",
    "Hipergranulação"
  ],
  "COBRIR_COM_OPCOES": [
    "Gaze úmida com SF 0,9% + gaze seca + chumaço + atadura e esparadrapo",
    "Gaze seca + chumaço + atadura e esparadrapo",
    "Gaze seca + atadura e esparadrapo",
    "Gaze seca + atadura e esparadrapo + atadura compressiva",
    "Ryon úmido com SF 0,9% + gaze seca + atadura e esparadrapo",
    "Ryon + gaze seca + atadura e esparadrapo"
  ],
  "ITB_INTERPRETACAO": [
    [
      1.41,
      99.0,
      "Falsamente elevado/calcificação"
    ],
    [
      1.0,
      1.4,
      "Normal"
    ],
    [
      0.91,
      0.99,
      "DAP limítrofe"
    ],
    [
      0.8,
      0.9,
      "DAP leve"
    ],
    [
      0.51,
      0.79,
      "DAP moderado"
    ],
    [
      0.0,
      0.5,
      "DAP severo"
    ]
  ]
};

export default DADOS;
